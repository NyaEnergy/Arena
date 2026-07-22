using System;
using System.Collections.Generic;

public sealed class CampaignService {
    private readonly CampaignConfig _config;
    private readonly CampaignRuntime _runtime;
    private readonly CampaignProgress _progress;
    private readonly GameplaySceneRequest _sceneRequest;

    public event Action<CommanderConfig> CommanderUnlocked;
    public event Action<StoryTaskConfig> StoryTaskCompleted;
    public event Action<StoryTerritoryConfig> TerritoryCompleted;
    public event Action<StoryArcConfig> ArcCompleted;

    public CampaignService(CampaignConfig config,
                           CampaignRuntime runtime,
                           CampaignProgress progress,
                           GameplaySceneRequest sceneRequest) {
        _config = config;
        _runtime = runtime;
        _progress = progress;
        _sceneRequest = sceneRequest;
    }

    public bool CanPrepare(StoryArcConfig storyArc) {
        if (storyArc == null ||
            !storyArc.IsValid ||
            _config == null ||
            !_config.IsValid) return false;

        int index = _config.IndexOf(storyArc);

        if (index < 0) return false;
        if (index == 0) return true;

        StoryArcConfig previousArc = _config.StoryArcs[index - 1];
        return _progress.IsCompleted(previousArc);
    }

    public bool TryPrepare(StoryArcConfig storyArc) {
        if ((_runtime.State != CampaignState.None &&
             _runtime.State != CampaignState.ArcCompleted) ||
            !CanPrepare(storyArc)) return false;

        Grant(storyArc.GrantedAlliedCommanders);
        Grant(storyArc.GrantedEnemyCommanders);
        _sceneRequest.Clear();
        _runtime.Prepare(storyArc);
        return true;
    }

    public bool TrySelectCommanders(AllyCommanderConfig alliedCommander,
                                    EnemyCommanderConfig enemyCommander) {
        if (_runtime.State != CampaignState.CommanderSelection ||
            alliedCommander == null ||
            enemyCommander == null ||
            alliedCommander.TeamType != TeamType.Ally ||
            enemyCommander.TeamType != TeamType.Enemy ||
            !_progress.IsOwned(alliedCommander) ||
            !_progress.IsOwned(enemyCommander)) return false;

        _runtime.SelectCommanders(alliedCommander, enemyCommander);
        return true;
    }

    public bool TryBeginTerritory() {
        StoryTerritoryConfig territory = _runtime.CurrentTerritory;

        if (_runtime.State != CampaignState.TerritoryReady ||
            territory == null ||
            !territory.IsValid) return false;

        if (!_sceneRequest.TryPrepare(
                _runtime.AlliedCommander,
                _runtime.EnemyCommander,
                territory)) return false;

        _runtime.BeginTerritory();
        return true;
    }

    public bool TryCompleteCurrentStoryTask(string storyTaskId) {
        StoryTaskConfig currentTask = _runtime.CurrentStoryTask;

        if (_runtime.State != CampaignState.TerritoryInProgress ||
            currentTask == null ||
            string.IsNullOrWhiteSpace(storyTaskId) ||
            !string.Equals(currentTask.Id, storyTaskId.Trim(),
                           StringComparison.Ordinal)) return false;

        StoryTaskCompleted?.Invoke(currentTask);

        StoryTerritoryConfig territory = _runtime.CurrentTerritory;
        bool isLastTask = _runtime.StoryTaskIndex >=
                          territory.StoryTasks.Count - 1;

        if (!isLastTask) {
            _runtime.AdvanceStoryTask();
            return true;
        }

        _progress.Complete(territory);

        bool isLastTerritory = _runtime.TerritoryIndex >=
                               _runtime.CurrentArc.Territories.Count - 1;

        StoryArcConfig completedArc = _runtime.CurrentArc;

        _runtime.CompleteTerritory(isLastTerritory);
        _sceneRequest.Clear();
        TerritoryCompleted?.Invoke(territory);

        if (!isLastTerritory) return true;

        _progress.Complete(completedArc);
        ArcCompleted?.Invoke(completedArc);
        return true;
    }

    public bool TryRestartTerritory() {
        if (_runtime.State != CampaignState.TerritoryInProgress) return false;

        _sceneRequest.Clear();
        _runtime.RestartTerritory();
        return true;
    }

    public void AbandonArc() {
        _sceneRequest.Clear();
        _runtime.Reset();
    }

    private void Grant<TCommander>(IReadOnlyList<TCommander> commanders)
                    where TCommander : CommanderConfig {

        if (commanders == null) return;

        for (int i = 0; i < commanders.Count; ++i) {
            TCommander commander = commanders[i];

            if (_progress.Unlock(commander)) {
                CommanderUnlocked?.Invoke(commander);
            }
        }
    }
}
