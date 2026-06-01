using System.Collections.Generic;
using UnityEngine;

public class EnemyPlatformPool {
    private readonly EnemyPlatformView _platformPrefab;
    private readonly Queue<EnemyPlatformView> _pool;

    public EnemyPlatformPool(EnemyPlatformView platformPrefab) {
        _platformPrefab = platformPrefab;
        _pool = new Queue<EnemyPlatformView>();
    }

    public EnemyPlatformView Get() {
        EnemyPlatformView platform;

        if(_pool.Count > 0)
            platform = _pool.Dequeue();
        else
            platform = Object.Instantiate(_platformPrefab);

        platform.Enable();

        return platform;
    }

    public void Return(EnemyPlatformView platform) {
        platform.Disable();
        _pool.Enqueue(platform);
    }
}
