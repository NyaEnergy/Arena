using System;

public sealed class CampaignRuntime {
    public event Action Changed;

    public CampaignState State { get; private set; }
    public StoryArcConfig CurrentArc { get; private set; }
    public AllyCommanderConfig AlliedCommander { get; private set; }
    public EnemyCommanderConfig EnemyCommander { get; private set; }
    public int TerritoryIndex { get; private set; } = -1;
    public int StoryTaskIndex { get; private set; } = -1;

    public StoryTerritoryConfig CurrentTerritory {
        get {
            if (CurrentArc == null ||
                TerritoryIndex < 0 ||
                TerritoryIndex >= CurrentArc.Territories.Count) return null;

            return CurrentArc.Territories[TerritoryIndex];
        }
    }

    public StoryTaskConfig CurrentStoryTask {
        get {
            StoryTerritoryConfig territory = CurrentTerritory;

            if (territory == null ||
                StoryTaskIndex < 0 ||
                StoryTaskIndex >= territory.StoryTasks.Count) return null;

            return territory.StoryTasks[StoryTaskIndex];
        }
    }

    internal void Prepare(StoryArcConfig storyArc) {
        CurrentArc = storyArc;
        AlliedCommander = null;
        EnemyCommander = null;
        TerritoryIndex = 0;
        StoryTaskIndex = -1;
        State = CampaignState.CommanderSelection;
        NotifyChanged();
    }

    internal void SelectCommanders(AllyCommanderConfig alliedCommander,
                                   EnemyCommanderConfig enemyCommander) {
        AlliedCommander = alliedCommander;
        EnemyCommander = enemyCommander;
        State = CampaignState.TerritoryReady;
        NotifyChanged();
    }

    internal void BeginTerritory() {
        StoryTaskIndex = 0;
        State = CampaignState.TerritoryInProgress;
        NotifyChanged();
    }

    internal void AdvanceStoryTask() {
        ++StoryTaskIndex;
        NotifyChanged();
    }

    internal void CompleteTerritory(bool isLastTerritory) {
        StoryTaskIndex = -1;

        if (isLastTerritory) {
            State = CampaignState.ArcCompleted;
        } else {
            ++TerritoryIndex;
            State = CampaignState.TerritoryReady;
        }

        NotifyChanged();
    }

    internal void RestartTerritory() {
        StoryTaskIndex = -1;
        State = CampaignState.TerritoryReady;
        NotifyChanged();
    }

    internal void Reset() {
        State = CampaignState.None;
        CurrentArc = null;
        AlliedCommander = null;
        EnemyCommander = null;
        TerritoryIndex = -1;
        StoryTaskIndex = -1;
        NotifyChanged();
    }

    private void NotifyChanged() {
        Changed?.Invoke();
    }
}
