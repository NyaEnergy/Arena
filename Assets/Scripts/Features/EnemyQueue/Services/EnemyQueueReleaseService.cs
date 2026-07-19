public class EnemyQueueReleaseService {
    private readonly CharacterDeploymentService _deploymentService;
    private readonly EnemyQueueService _queueService;
    private readonly TerritorySpawnGate _spawnGate;

    public EnemyQueueReleaseService(CharacterDeploymentService deploymentService,
                                    EnemyQueueService queueService,
                                    TerritorySpawnGate spawnGate) {
        _deploymentService = deploymentService;
        _queueService = queueService;
        _spawnGate = spawnGate;
    }

    public bool ReleaseNext() {
        if (!_queueService.TryPeek(
                out EnemyQueueItem item)) {
            return false;
        }

        if (!_spawnGate.TryGetEnemyPosition(
                out UnityEngine.Vector3 position)) {
            return false;
        }

        CharacterView view =
            _deploymentService.Deploy(
                item.CreateRequest(),
                position);

        if (view == null) return false;

        return _queueService.RemoveFirst();
    }
}