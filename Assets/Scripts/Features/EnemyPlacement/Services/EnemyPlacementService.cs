using UnityEngine;

public class EnemyPlacementService {
    private readonly TerritoryDropService _dropService;
    private readonly TerritorySpawnGate _spawnGate;

    private readonly EnemyGroupDeploymentService
        _groupDeploymentService;

    public EnemyPlacementService(
                TerritoryDropService dropService,
                TerritorySpawnGate spawnGate,
                EnemyGroupDeploymentService groupDeploymentService) {

        _dropService = dropService;
        _spawnGate = spawnGate;
        _groupDeploymentService = groupDeploymentService;
    }

    public bool TryPlace(EnemyQueueItem item,
                         Vector2 screenPosition) {

        if (item == null || !item.IsValid) return false;

        if (!_dropService.TryGet(screenPosition,
                             out TerritoryRuntime territory,
                             out Vector3 position)) {
            return false;
        }

        if (!_spawnGate.CanSpawn(territory)) return false;

        return _groupDeploymentService.TryDeploy(
            item, territory, position);
    }
}