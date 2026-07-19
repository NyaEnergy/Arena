using UnityEngine;
using Zenject;

public sealed class EnemyConveyorService : IInitializable,
                                           ITickable {
    private readonly EnemyConveyorConfig _config;
    private readonly EnemyConveyorRuntime _runtime;
    private readonly EnemyConveyorSource _source;
    private readonly EnemyQueueRuntime _queueRuntime;
    private readonly EnemyQueueService _queueService;
    private readonly EnemyQueueReleaseService _releaseService;
    private readonly IQueueDragState _dragState;

    public EnemyConveyorService(EnemyConveyorConfig config,
                                EnemyConveyorRuntime runtime,
                                EnemyConveyorSource source,
                                EnemyQueueRuntime queueRuntime,
                                EnemyQueueService queueService,
                                EnemyQueueReleaseService releaseService,
                                IQueueDragState dragState) {
        _config = config;
        _runtime = runtime;
        _source = source;
        _queueRuntime = queueRuntime;
        _queueService = queueService;
        _releaseService = releaseService;
        _dragState = dragState;
    }

    public void Initialize() {
        if (_config == null) return;

        _runtime.Reset(Time.time,
                       _config.StartDelay);
    }

    public void Tick() {
        if (_config == null ||
            _dragState.IsDragging ||
            !_runtime.IsReady(Time.time)) {
            return;
        }

        _runtime.Schedule(Time.time,
                          _config.FeedInterval);

        TryFeed();
    }

    private bool TryFeed() {
        if (!_source.TryGetNext(out EnemyQueueItem item,
                                out int entryIndex,
                                out int entryCount)) {
            return false;
        }

        if (_queueRuntime.IsFull &&
            !_releaseService.ReleaseNext()) {
            return false;
        }

        if (!_queueService.TryAdd(item)) {
            return false;
        }

        _source.Confirm(entryIndex, entryCount);
        return true;
    }
}
