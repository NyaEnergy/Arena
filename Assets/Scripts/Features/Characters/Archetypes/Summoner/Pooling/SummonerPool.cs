using System.Collections.Generic;
using UnityEngine;

public class SummonerPool {
    private const int DEFAULT_PRELOAD_COUNT = 2;

    private readonly SummonerConfigCollection _configCollection;
    private readonly SummonerInstanceFactory _instanceFactory;

    private readonly Dictionary<SummonerPoolKey, Queue<SummonerView>> _pools = new();
    private readonly Dictionary<SummonerView, SummonerPoolKey> _keys = new();

    private readonly HashSet<SummonerView> _pooledViews = new();

    public SummonerPool(SummonerConfigCollection configCollection,
                        SummonerInstanceFactory instanceFactory) {

        _configCollection = configCollection;
        _instanceFactory = instanceFactory;
    }

    public SummonerView Get(SummonerPoolKey key,
                            Vector3 position) {

        if (!_configCollection.Contains(key.Config))
            return null;

        Queue<SummonerView> pool = GetPool(key);

        while (pool.Count < DEFAULT_PRELOAD_COUNT) {
            SummonerView created = _instanceFactory.Create(key, Return);

            if (created == null) break;

            _keys.Add(created, key);
            Return(created);
        }

        if (pool.Count == 0) return null;

        SummonerView view = pool.Dequeue();
        _pooledViews.Remove(view);

        view.transform.SetPositionAndRotation(
            position, Quaternion.identity);

        view.OnSpawned();
        return view;
    }

    public void Return(CharacterView character) {
        SummonerView view = character as SummonerView;

        if (view == null ||
            !_keys.TryGetValue(view, out SummonerPoolKey key) ||
            !_pooledViews.Add(view))
                return;

        view.OnDespawned();
        GetPool(key).Enqueue(view);
    }

    private Queue<SummonerView> GetPool(SummonerPoolKey key) {
        if (_pools.TryGetValue(key, out Queue<SummonerView> pool)) {
            return pool;
        }

        pool = new Queue<SummonerView>();
        _pools.Add(key, pool);

        return pool;
    }
}