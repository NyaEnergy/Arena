using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class EnemyPlatformPool {
    private readonly DiContainer _container;
    private readonly Queue<EnemyPlatformView> _pool;
    private readonly EnemyPlatformView _prefab;

    public EnemyPlatformPool(DiContainer container, EnemyPlatformView prefab) {
        _pool = new Queue<EnemyPlatformView>();
        _container = container;
        _prefab = prefab;
    }

    public EnemyPlatformView Get() {
        EnemyPlatformView platform;

        if (_pool.Count > 0)
            platform = _pool.Dequeue();
        else
            platform = _container.InstantiatePrefabForComponent<EnemyPlatformView>(_prefab);

        platform.gameObject.SetActive(true);

        return platform;
    }

    public void Return(EnemyPlatformView platform) {
        platform.gameObject.SetActive(false);
        _pool.Enqueue(platform);
    }
}
