using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SummonedCharacterPool {
    private const int DEFAULT_PRELOAD_COUNT = 4;

    private readonly DiContainer _container;
    private readonly CharacterLifecycleFactory _lifecycleFactory;

    private readonly Dictionary<SummonedCharacterPoolKey, Queue<CharacterView>> _pools = new();
    private readonly Dictionary<CharacterView, SummonedCharacterPoolKey> _keys = new();

    private readonly HashSet<CharacterView> _pooledViews = new();

    public SummonedCharacterPool(DiContainer container,
                                 CharacterLifecycleFactory lifecycleFactory) {
        _container = container;
        _lifecycleFactory = lifecycleFactory;
    }

    public CharacterView Get(SummonedCharacterPoolKey key,
                             Vector3 position) {

        if (key.Config == null) return null;

        Queue<CharacterView> pool = GetPool(key);

        while (pool.Count < DEFAULT_PRELOAD_COUNT) {
            CharacterView created = Create(key);

            if (created == null) break;

            Return(created);
        }

        if (pool.Count == 0) return null;

        CharacterView view = pool.Dequeue();

        _pooledViews.Remove(view);

        view.transform.SetPositionAndRotation(
            position, Quaternion.identity);

        view.OnSpawned();
        return view;
    }

    public void Return(CharacterView view) {
        if (view == null ||
            !_keys.TryGetValue(view, out SummonedCharacterPoolKey key) ||
            !_pooledViews.Add(view)) {
            return;
        }

        view.OnDespawned();
        GetPool(key).Enqueue(view);
    }

    private Queue<CharacterView> GetPool(SummonedCharacterPoolKey key) {
        if (_pools.TryGetValue(
                key, out Queue<CharacterView> pool)) {

            return pool;
        }

        pool = new Queue<CharacterView>();
        _pools.Add(key, pool);
        return pool;
    }

    private CharacterView Create(SummonedCharacterPoolKey key) {
        CharacterView prefab = key.Config.Prefab;

        if (prefab == null) return null;

        CharacterView view = _container.InstantiatePrefabForComponent<CharacterView>(prefab);

        CharacterLifecycleController controller =
            _lifecycleFactory.Create(
                view,
                key.TeamType,
                key.Config,
                null,
                Return);

        if (controller == null ||
           !view.Initialize(controller)) {
                Object.Destroy(view.gameObject);
                return null;
        }

        _keys.Add(view, key);
        return view;
    }
}