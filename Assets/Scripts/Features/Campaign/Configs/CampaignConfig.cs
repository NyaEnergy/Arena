using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Campaign/Campaign Config",
                 fileName = "CampaignConfig")]
public sealed class CampaignConfig : ScriptableObject {
    [SerializeField] private List<StoryArcConfig> _storyArcs = new();

    public IReadOnlyList<StoryArcConfig> StoryArcs => _storyArcs;

    public int IndexOf(StoryArcConfig storyArc) {
        if (storyArc == null || _storyArcs == null) return -1;
        return _storyArcs.IndexOf(storyArc);
    }

    public bool IsValid {
        get {
            if (_storyArcs == null || _storyArcs.Count == 0) return false;

            HashSet<string> arcIds = new(StringComparer.Ordinal);
            HashSet<string> territoryIds = new(StringComparer.Ordinal);
            HashSet<string> storyTaskIds = new(StringComparer.Ordinal);
            HashSet<string> commanderIds = new(StringComparer.Ordinal);

            for (int i = 0; i < _storyArcs.Count; ++i) {
                StoryArcConfig storyArc = _storyArcs[i];

                if (storyArc == null ||
                    !storyArc.IsValid ||
                    !arcIds.Add(storyArc.Id) ||
                    !AddCommanderIds(storyArc.GrantedAlliedCommanders, commanderIds) ||
                    !AddCommanderIds(storyArc.GrantedEnemyCommanders, commanderIds) ||
                    !AddTerritoryIds(storyArc.Territories, territoryIds, storyTaskIds)) return false;
            }

            return true;
        }
    }

    private static bool AddCommanderIds<TCommander>(
                            IReadOnlyList<TCommander> commanders,
                            HashSet<string> ids) where TCommander : CommanderConfig {

        for (int i = 0; i < commanders.Count; ++i) {
            if (!ids.Add(commanders[i].Id)) return false;
        }

        return true;
    }

    private static bool AddTerritoryIds(
        IReadOnlyList<StoryTerritoryConfig> territories,
        HashSet<string> territoryIds,
        HashSet<string> storyTaskIds) {
        for (int i = 0; i < territories.Count; ++i) {
            StoryTerritoryConfig territory = territories[i];

            if (!territoryIds.Add(territory.Id)) return false;

            for (int taskIndex = 0;
                    taskIndex < territory.StoryTasks.Count;
                        ++taskIndex) {
                if (!storyTaskIds.Add(territory.StoryTasks[taskIndex].Id)) return false;
            }
        }

        return true;
    }
}
