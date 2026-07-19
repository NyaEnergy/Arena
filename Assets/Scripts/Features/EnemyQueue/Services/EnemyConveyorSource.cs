using System.Collections.Generic;

public sealed class EnemyConveyorSource {
    private readonly EnemyConveyorConfig _config;
    private readonly EnemyConveyorRuntime _runtime;

    public EnemyConveyorSource(EnemyConveyorConfig config,
                               EnemyConveyorRuntime runtime) {
        _config = config;
        _runtime = runtime;
    }

    public bool TryGetNext(out EnemyQueueItem item,
                           out int entryIndex,
                           out int entryCount) {
        item = null;
        entryIndex = -1;

        IReadOnlyList<EnemyConveyorEntry> entries = _config?.Entries;

        entryCount = entries?.Count ?? 0;
        if (entryCount == 0) return false;

        int startIndex = _runtime.NextEntryIndex % entryCount;

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

    public void Confirm(int entryIndex, int entryCount) {
        _runtime.Advance(entryIndex, entryCount);
    }
}