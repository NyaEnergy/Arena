public class EnemyQueueService {
    private readonly EnemyQueueRuntime _runtime;

    public EnemyQueueService(EnemyQueueRuntime runtime) {
        _runtime = runtime;
    }

    public bool TryAdd(EnemyQueueItem item) {
        return _runtime.TryAdd(item);
    }

    public bool TryPeek(out EnemyQueueItem item) {
        return _runtime.TryPeek(out item);
    }

    public bool RemoveFirst() {
        return _runtime.RemoveFirst();
    }

    public bool RemoveAt(int index) {
        return _runtime.RemoveAt(index);
    }
}