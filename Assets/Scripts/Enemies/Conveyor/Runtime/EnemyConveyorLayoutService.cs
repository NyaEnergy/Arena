using System;
using UnityEngine;

public class EnemyConveyorLayoutService {
    private readonly EnemyConveyorRuntime _runtime;
    private readonly EnemyConveyorRoot _root;

    public EnemyConveyorLayoutService(EnemyConveyorRuntime runtime,
                                      EnemyConveyorRoot root) {
        _runtime = runtime;
        _root = root;
    }

    public void RefreshLayout() {
        for (int i = 0; i < _runtime.Count; ++i) {
            EnemyConveyorSlotRuntime slot = _runtime.Get(i);
            Vector3 targetPosition = CalculatePlatformPosition(i);
            slot.Platform.MoveTo(targetPosition);
        }
    }

    private Vector3 CalculatePlatformPosition(int index) {
        return _root.SpawnPoint.position +
               _root.RightDirection *
               (_root.PlatformSpacing * index);
    }
}
