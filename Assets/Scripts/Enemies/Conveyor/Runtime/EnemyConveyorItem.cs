using UnityEngine;

public class EnemyConveyorItem : MonoBehaviour {
    private readonly EnemyPlatformView _platformView;

    public EnemyPlatformView PlatformView => _platformView;

    public EnemyConveyorItem(EnemyPlatformView platformView) {
        _platformView = platformView;
    }
}
