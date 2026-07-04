using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterPool {
    private const int DEFAULT_PRELOAD_COUNT = 4;

    private readonly DiContainer _container;
    private readonly CharacterConfigRegistry _configRegistry;
    private readonly CharacterPrefabRegistry _prefabRegistry;
    private readonly CharacterControllerFactory _controllerFactory;

    private readonly Dictionary<CharacterKey, Queue<CharacterView>> _pools = new();
    private readonly HashSet<CharacterView> _pooledCharacters = new();

    public CharacterPool(DiContainer container,
                         CharacterConfigRegistry configRegistry,
                         CharacterPrefabRegistry prefabRegistry,
                         CharacterControllerFactory controllerFactory) {
        _container = container;
        _configRegistry = configRegistry;
        _prefabRegistry = prefabRegistry;
        _controllerFactory = controllerFactory;
    }

    public void Warmup(CharacterKey key,
                       int preloadCount) {

        if (preloadCount <= 0) return;

        Queue<CharacterView> pool = GetOrCreatePool(key);

        while (pool.Count < preloadCount) {
            CharacterView character = CreateInstance(key);

            if (character == null) break;

            Return(character);
        }
    }

    public CharacterView Get(CharacterKey key,
                             Vector3 position) {

        Queue<CharacterView> pool = GetOrCreatePool(key);

        if (pool.Count == 0) {
            Warmup(key, DEFAULT_PRELOAD_COUNT);
        }

        if (pool.Count == 0) return null;

        CharacterView character = pool.Dequeue();
        _pooledCharacters.Remove(character);

        character.transform
            .SetPositionAndRotation(
                position, Quaternion.identity);

        character.OnSpawned();

        return character;
    }

    public void Return(CharacterView character) {

        if (character == null) return;
        if (!_pooledCharacters.Add(character)) return;

        character.OnDespawned();

        Queue<CharacterView> pool =
            GetOrCreatePool(character.CharacterKey);

        pool.Enqueue(character);
    }

    private Queue<CharacterView>
        GetOrCreatePool(CharacterKey key) {

        if (_pools.TryGetValue(key, out Queue<CharacterView> pool)) {
            return pool;
        }

        pool = new Queue<CharacterView>();

        _pools.Add(key, pool);

        return pool;
    }

    private CharacterView CreateInstance(CharacterKey key) {

        ICharacterConfig config =
            _configRegistry.Get(key);

        CharacterView prefab =
            _prefabRegistry.Get(key);

        if (config == null ||
            prefab == null) {

            return null;
        }

        CharacterView character =
            _container.InstantiatePrefabForComponent<CharacterView>(prefab.gameObject);

        CharacterController controller = _controllerFactory.Create(
                character, key, config, Return);

        if (controller == null || !character.Initialize(controller)) {

            Object.Destroy(character.gameObject);
            return null;
        }

        return character;
    }
}