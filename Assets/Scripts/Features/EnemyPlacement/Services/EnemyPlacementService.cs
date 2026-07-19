using UnityEngine;

public class EnemyPlacementService {
    private readonly TerritoryDropService _dropService;
    private readonly TerritorySpawnGate _spawnGate;

    private readonly CharacterDeploymentService
        _deploymentService;

    public EnemyPlacementService(
                TerritoryDropService dropService,
                TerritorySpawnGate spawnGate,
                CharacterDeploymentService deploymentService) {

        _dropService = dropService;
        _spawnGate = spawnGate;
        _deploymentService = deploymentService;
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

        CharacterView view = _deploymentService.Deploy(
                                item.CreateRequest(),
                                position);

        return view != null;
    }
}