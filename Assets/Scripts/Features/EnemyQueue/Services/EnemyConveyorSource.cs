using System.Collections.Generic;

public sealed class EnemyConveyorSource {
    private readonly EnemyCommanderConfig _config;
    private readonly EnemyConveyorRuntime _runtime;

    public EnemyConveyorSource(
            EnemyCommanderConfig config,
            EnemyConveyorRuntime runtime) {
        _config = config;
        _runtime = runtime;
    }

    public bool TryGetNext(out EnemyQueueItem item,
                           out int groupIndex,
                           out int groupCount) {

        item = null;
        groupIndex = -1;

        IReadOnlyList<EnemyGroupConfig> groups =
            _config?.Groups;

        groupCount = groups?.Count ?? 0;

        if (groupCount == 0) return false;

        int startIndex = _runtime.NextGroupIndex % groupCount;

        for (int offset = 0; offset < groupCount; offset++) {

            int index = (startIndex + offset) % groupCount;
            EnemyQueueItem candidate = groups[index]?.CreateItem();

            if (candidate == null) continue;

            item = candidate;
            groupIndex = index;

            return true;
        }

        return false;
    }

    public void Confirm(int groupIndex,
                        int groupCount) {
        _runtime.ConfirmGroup(groupIndex, groupCount);
    }
}