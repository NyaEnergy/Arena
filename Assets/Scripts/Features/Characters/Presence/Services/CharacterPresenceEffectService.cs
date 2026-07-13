using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterPresenceEffectService : ITickable {
    private const float DEFAULT_LIFETIME = 0.5f;

    private readonly DiContainer _container;
    private readonly Dictionary<ParticleSystem, Queue<ParticleSystem>> _pools = new();
    private readonly List<CharacterPresenceEffectRuntime> _activeEffects = new();

    public CharacterPresenceEffectService(DiContainer container) {
        _container = container;
    }

    public void Tick() {
        for (int i = _activeEffects.Count - 1; i >= 0; i--) {
            CharacterPresenceEffectRuntime runtime = _activeEffects[i];
            runtime.RemainingTime -= Time.deltaTime;

            if (runtime.RemainingTime > 0f) continue;

            Return(runtime.Prefab, runtime.Instance);
            _activeEffects.RemoveAt(i);
        }
    }

    public void Play(ParticleSystem prefab,
                     Vector3 position,
                     Quaternion rotation) {
        if (prefab == null) return;

        ParticleSystem instance = Get(prefab);

        ParticleSystem[] systems =
            instance.GetComponentsInChildren<ParticleSystem>(true);

        instance.transform.SetPositionAndRotation(position, rotation);
        instance.gameObject.SetActive(true);

        for (int i = 0; i < systems.Length; i++) {
            systems[i].Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        instance.Play(true);

        _activeEffects.Add(
            new CharacterPresenceEffectRuntime(prefab,
                                               instance,
                                               GetLifetime(systems)));
    }

    private ParticleSystem Get(ParticleSystem prefab) {
        Queue<ParticleSystem> pool = GetPool(prefab);

        return pool.Count > 0 ?
            pool.Dequeue() :
            _container.InstantiatePrefabForComponent<ParticleSystem>(prefab);
    }

    private void Return(ParticleSystem prefab,
                        ParticleSystem instance) {
        if (instance == null) return;

        instance.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        instance.gameObject.SetActive(false);
        GetPool(prefab).Enqueue(instance);
    }

    private Queue<ParticleSystem> GetPool(ParticleSystem prefab) {
        if (_pools.TryGetValue(prefab, out Queue<ParticleSystem> pool)) {
            return pool;
        }

        pool = new Queue<ParticleSystem>();
        _pools.Add(prefab, pool);

        return pool;
    }

    private float GetLifetime(ParticleSystem[] systems) {
        float lifetime = 0f;

        for (int i = 0; i < systems.Length; i++) {
            ParticleSystem.MainModule main = systems[i].main;

            float value = main.startDelay.constantMax +
                          main.duration +
                          main.startLifetime.constantMax;

            lifetime = Mathf.Max(lifetime, value);
        }

        return lifetime > 0f ?
            lifetime : DEFAULT_LIFETIME;
    }
}