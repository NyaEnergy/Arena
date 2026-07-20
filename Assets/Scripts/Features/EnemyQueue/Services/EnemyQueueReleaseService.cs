public class EnemyQueueReleaseService {
    private readonly EnemyGroupDeploymentService _deploymentService;
    private readonly EnemyQueueService _queueService;
    private readonly TerritorySpawnGate _spawnGate;

    public EnemyQueueReleaseService(EnemyGroupDeploymentService deploymentService,
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
                out TerritoryRuntime territory,
                out UnityEngine.Vector3 position)) {
            return false;
        }

        if (!_deploymentService.TryDeploy(
                item, territory, position)) {
            return false;
        }

        return _queueService.RemoveFirst();
    }
}