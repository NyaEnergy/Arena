using System.Collections.Generic;

public sealed class EnemyConveyorSource {
    private readonly EnemyDirectorConfig _directorConfig;
    private readonly EnemyDirectorRuntime _directorRuntime;
    private readonly EnemyDirectorService _directorService;

    public EnemyConveyorSource(
            EnemyDirectorConfig directorConfig,
            EnemyDirectorRuntime directorRuntime,
            EnemyDirectorService directorService) {

        _directorConfig = directorConfig;
        _directorRuntime = directorRuntime;
        _directorService = directorService;
    }

    public bool TryGetNext(out EnemyQueueItem item,
                           out EnemyDirectorState state,
                           out int entryIndex,
                           out int entryCount) {

        item = null;
        state = _directorService.State;
        entryIndex = -1;

        EnemyDirectorProfile profile =
            _directorConfig?.GetProfile(state);

        IReadOnlyList<EnemyConveyorEntry> entries =
            profile?.Entries;

        entryCount = entries?.Count ?? 0;

        if (entryCount == 0) return false;

        int startIndex =
            _directorRuntime
                .GetNextEntryIndex(state) % entryCount;

        for (int offset = 0; offset < entryCount; offset++) {

            int index = (startIndex + offset) % entryCount;
            EnemyConveyorEntry entry = entries[index];
            EnemyQueueItem candidate = entry?.CreateItem();

            if (candidate == null) continue;

            item = candidate;
            entryIndex = index;

            return true;
        }

        return false;
    }

    public void Confirm(EnemyDirectorState state,
                        int entryIndex,
                        int entryCount) {

        _directorRuntime.Advance(
            state, entryIndex, entryCount);
    }
}