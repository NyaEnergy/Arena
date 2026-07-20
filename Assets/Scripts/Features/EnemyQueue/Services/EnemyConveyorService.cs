using UnityEngine;
using Zenject;

public sealed class EnemyConveyorService : IInitializable,
                                           ITickable {
    private readonly EnemyCommanderConfig _config;
    private readonly EnemyConveyorRuntime _runtime;
    private readonly EnemyConveyorSource _source;

    private readonly EnemyQueueRuntime _queueRuntime;
    private readonly EnemyQueueService _queueService;

    private readonly EnemyDirectorService _directorService;
    private readonly IQueueDragState _dragState;

    public EnemyConveyorService(
                EnemyCommanderConfig config,
                EnemyConveyorRuntime runtime,
                EnemyConveyorSource source,
                EnemyQueueRuntime queueRuntime,
                EnemyQueueService queueService,
                EnemyDirectorService directorService,
                IQueueDragState dragState) {

        _config = config;
        _runtime = runtime;
        _source = source;

        _queueRuntime = queueRuntime;
        _queueService = queueService;

        _directorService = directorService;
        _dragState = dragState;
    }

    public void Initialize() {
        if (_config == null) return;

        _runtime.Reset(_config.StartDelay);
    }

    public void Tick() {
        if (_config == null) return;

        _runtime.Tick(Time.deltaTime);

        if (_dragState.IsDragging ||
            _queueRuntime.IsFull ||
            !_directorService.CanRefill ||
            !_runtime.TryConsumeFeed(
                Time.deltaTime,
                _directorService.FeedInterval)) {
            return;
        }

        TryFeed();
    }

    private bool TryFeed() {
        if (!_source.TryGetNext(out EnemyQueueItem item,
                                out int groupIndex,
                                out int groupCount)) {
            return false;
        }

        if (!_queueService.TryAdd(item)) {
            return false;
        }

        _source.Confirm(groupIndex, groupCount);

        return true;
    }
}
