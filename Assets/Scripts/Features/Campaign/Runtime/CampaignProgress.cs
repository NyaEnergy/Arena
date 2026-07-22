using System;
using System.Collections.Generic;

public sealed class CampaignProgress {
    private readonly List<CommanderConfig> _ownedCommanders = new();
    private readonly HashSet<string> _completedArcIds = new(StringComparer.Ordinal);
    private readonly HashSet<string> _completedTerritoryIds = new(StringComparer.Ordinal);

    public IReadOnlyList<CommanderConfig> OwnedCommanders => _ownedCommanders;

    public bool Unlock(CommanderConfig commander) {
        if (commander == null ||
           !commander.IsValid ||
           IsOwned(commander)) return false;

        _ownedCommanders.Add(commander);
        return true;
    }

    public bool IsOwned(CommanderConfig commander) {
        if (commander == null || !commander.IsValid) return false;

        for (int i = 0; i < _ownedCommanders.Count; ++i) {
            CommanderConfig owned = _ownedCommanders[i];

            if (owned != null &&
                string.Equals(owned.Id, commander.Id,
                              StringComparison.Ordinal)) return true;
        }

        return false;
    }

    public bool Complete(StoryTerritoryConfig territory) {
        return territory != null &&
               territory.IsValid &&
               _completedTerritoryIds.Add(territory.Id);
    }

    public bool Complete(StoryArcConfig storyArc) {
        return storyArc != null &&
               storyArc.IsValid &&
               _completedArcIds.Add(storyArc.Id);
    }

    public bool IsCompleted(StoryTerritoryConfig territory) {
        return territory != null &&
               territory.IsValid &&
               _completedTerritoryIds.Contains(territory.Id);
    }

    public bool IsCompleted(StoryArcConfig storyArc) {
        return storyArc != null &&
               storyArc.IsValid &&
               _completedArcIds.Contains(storyArc.Id);
    }
}
