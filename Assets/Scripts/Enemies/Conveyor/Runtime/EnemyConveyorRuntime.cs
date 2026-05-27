using System.Collections.Generic;

public class EnemyConveyorRuntime {
    private readonly List<EnemySpawnRequest> _queue = new();

    public IReadOnlyList<EnemySpawnRequest> Queue => _queue;

    public void Add(EnemySpawnRequest request) {
        _queue.Add(request);
    }

    public EnemySpawnRequest PopFront() {
        EnemySpawnRequest request = _queue[0];
        _queue.RemoveAt(0);
        return request;
    }

    public bool IsFull(int maxCount) {
        return _queue.Count >= maxCount;
    }

    public bool HasEnemies() {
        return _queue.Count > 0;
    }
}
