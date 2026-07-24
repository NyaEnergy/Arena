using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class StoryTaskService : IInitializable,
                                       ITickable,
                                       IDisposable {
    private readonly CampaignRuntime _campaignRuntime;
    private readonly CampaignService _campaignService;
    private readonly CharacterDeathEventService _deathEventService;
    private readonly BattlefieldRegistry _battlefield;
    private readonly StoryTaskRuntime _runtime;

    public StoryTaskService(
        CampaignRuntime campaignRuntime,
        CampaignService campaignService,
        CharacterDeathEventService deathEventService,
        BattlefieldRegistry battlefield,
        StoryTaskRuntime runtime) {

        _campaignRuntime = campaignRuntime ??
            throw new ArgumentNullException(
                nameof(campaignRuntime));
        _campaignService = campaignService ??
            throw new ArgumentNullException(
                nameof(campaignService));
        _deathEventService = deathEventService ??
            throw new ArgumentNullException(
                nameof(deathEventService));
        _battlefield = battlefield ??
            throw new ArgumentNullException(
                nameof(battlefield));
        _runtime = runtime ??
            throw new ArgumentNullException(nameof(runtime));
    }

    public void Initialize() {
        _campaignRuntime.Changed +=
            OnCampaignRuntimeChanged;
        _deathEventService.CharacterDied +=
            OnCharacterDied;

        SynchronizeTask();
    }

    public void Tick() {
        StoryTaskConfig task = _runtime.Task;

        if (_runtime.Phase != StoryTaskPhase.Active ||
            task == null ||
            task.ObjectiveType !=
            StoryTaskObjectiveType.SurviveSeconds) return;

        _runtime.AddProgress(Time.deltaTime);
    }

    public void Dispose() {
        _campaignRuntime.Changed -=
            OnCampaignRuntimeChanged;
        _deathEventService.CharacterDied -=
            OnCharacterDied;
    }

    public bool TryCompleteDialogue() {
        if (_runtime.Phase == StoryTaskPhase.Intro) {
            return _runtime.TryBegin();
        }

        if (_runtime.Phase != StoryTaskPhase.Outro ||
            _runtime.Task == null) return false;

        if (_runtime.IsLastTask) {
            return _runtime.TryShowVictory();
        }

        return _campaignService
            .TryCompleteCurrentStoryTask(
                _runtime.Task.Id);
    }

    public bool TryConfirmVictory() {
        if (_runtime.Phase != StoryTaskPhase.Victory ||
            _runtime.Task == null) return false;

        return _campaignService
            .TryCompleteCurrentStoryTask(
                _runtime.Task.Id);
    }

    public bool TryRestart() {
        return _runtime.Phase == StoryTaskPhase.Defeat &&
               _campaignService.TryRestartTerritory();
    }

    private void OnCampaignRuntimeChanged() {
        SynchronizeTask();
    }

    private void OnCharacterDied(
        CharacterDeathInfo deathInfo) {

        if (_runtime.Phase !=
            StoryTaskPhase.Active) return;

        if (deathInfo.TeamType == TeamType.Ally) {
            if (!HasLivingAllies()) {
                _runtime.Fail();
            }

            return;
        }

        StoryTaskConfig task = _runtime.Task;

        if (task != null &&
            task.ObjectiveType ==
            StoryTaskObjectiveType.DefeatEnemies) {
            _runtime.AddProgress(1f);
        }
    }

    private void SynchronizeTask() {
        StoryTaskConfig currentTask =
            _campaignRuntime.CurrentStoryTask;

        if (_campaignRuntime.State !=
                CampaignState.TerritoryInProgress ||
            currentTask == null) {
            _runtime.Reset();
            return;
        }

        if (_runtime.IsCurrent(currentTask.Id)) {
            return;
        }

        StoryTerritoryConfig territory =
            _campaignRuntime.CurrentTerritory;

        bool isLastTask =
            territory != null &&
            _campaignRuntime.StoryTaskIndex >=
            territory.StoryTasks.Count - 1;

        _runtime.Prepare(currentTask, isLastTask);
    }

    private bool HasLivingAllies() {
        IReadOnlyList<CharacterBrain> allies =
            _battlefield.Allies;

        for (int i = 0; i < allies.Count; ++i) {
            CharacterBrain ally = allies[i];

            if (ally != null &&
                ally.Runtime != null &&
                !ally.Runtime.IsDead.CurrentValue &&
                ally.Config is ICharacterConfig) {
                return true;
            }
        }

        return false;
    }
}
