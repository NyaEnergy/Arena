using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterPool {
    private const int DEFAULT_PRELOAD_COUNT = 4;

    private readonly DiContainer _container;
    private readonly CharacterConfigRegistry _configRegistry;
    private readonly CharacterLifecycleFactory _lifecycleFactory;
    private readonly CharacterDeathEventService _deathEventService;

    private readonly Dictionary<CharacterKey, Queue<CharacterView>> _pools = new();
    private readonly Dictionary<CharacterView, CharacterKey> _keys = new();

    private readonly HashSet<CharacterView> _pooledCharacters = new();

    public CharacterPool(DiContainer container,
                         CharacterConfigRegistry configRegistry,
                         CharacterLifecycleFactory lifecycleFactory,
                         CharacterDeathEventService deathEventService) {
        _container = container;
        _configRegistry = configRegistry;
        _lifecycleFactory = lifecycleFactory;
        _deathEventService = deathEventService;
    }

    public CharacterView Get(CharacterKey key,
                             Vector3 position) {
        Queue<CharacterView> pool = GetOrCreatePool(key);

        while (pool.Count < DEFAULT_PRELOAD_COUNT) {
            CharacterView created = CreateInstance(key);

            if (created == null) break;

            Return(created);
        }

        if (pool.Count == 0) return null;

        CharacterView character = pool.Dequeue();

        _pooledCharacters.Remove(character);

        character.transform.SetPositionAndRotation(
            position, Quaternion.identity);

        character.OnSpawned();

        return character;
    }

    public void Return(CharacterView character) {
        if (character == null ||
            !_keys.TryGetValue(character,
                           out CharacterKey key)) {
            return;
        }

        if (!_pooledCharacters.Add(character)) {
            return;
        }

        character.OnDespawned();
        GetOrCreatePool(key).Enqueue(character);
    }

    private Queue<CharacterView> GetOrCreatePool(CharacterKey key) {
        if (_pools.TryGetValue(
                key, out Queue<CharacterView> pool)) {
            return pool;
        }

        pool = new Queue<CharacterView>();
        _pools.Add(key, pool);

        return pool;
    }

    private CharacterView CreateInstance(CharacterKey key) {
        ICharacterConfig config = _configRegistry.Get(key);

        CharacterView prefab = config?.Prefab;

        if (prefab == null) return null;

        CharacterView character =
            _container.InstantiatePrefabForComponent<CharacterView>(prefab);

        CharacterLifecycleController controller = _lifecycleFactory.Create(
                character,
                key.TeamType,
                config,
                view => NotifyDeath(key, view),
                Return);

        if (controller == null ||
            !character.Initialize(controller)) {
            Object.Destroy(character.gameObject);

            return null;
        }

        _keys.Add(character, key);
        return character;
    }

    private void NotifyDeath(CharacterKey key,
                             CharacterView view) {
        _deathEventService.NotifyDeath(
            new CharacterDeathInfo(
                key, view.transform.position));
    }
}