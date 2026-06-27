using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterPool {
    private const int DEFAULT_PRELOAD_COUNT = 4;

    private readonly DiContainer _container;
    private readonly CharacterConfigRegistry _configRegistry;

    private readonly Dictionary<CharacterKey, Queue<BattlefieldCharacter>> _pools = new();
    private readonly HashSet<BattlefieldCharacter> _pooledCharacters = new();

    public CharacterPool(DiContainer container,
                         CharacterConfigRegistry configRegistry) {
        _container = container;
        _configRegistry = configRegistry;
    }

    public void Warmup(CharacterKey key,
                       int preloadCount) {
        if (preloadCount <= 0) return;

        Queue<BattlefieldCharacter> pool = GetOrCreatePool(key);

        while (pool.Count < preloadCount) {
            BattlefieldCharacter character = CreateInstance(key);
            Return(character);
        }
    }

    public BattlefieldCharacter Get(CharacterKey key,
                                    Vector3 position) {
        Queue<BattlefieldCharacter> pool = GetOrCreatePool(key);

        if (pool.Count == 0) Warmup(key, DEFAULT_PRELOAD_COUNT);

        BattlefieldCharacter character = pool.Dequeue();

        _pooledCharacters.Remove(character);

        character.transform.SetPositionAndRotation(position, Quaternion.identity);
        character.OnSpawned();

        return character;
    }

    public void Return(BattlefieldCharacter character) {
        if (character == null) return;

        if (!_pooledCharacters.Add(character)) return;

        character.OnDespawned();

        Queue<BattlefieldCharacter> pool = GetOrCreatePool(character.CharacterKey);
        pool.Enqueue(character);
    }

    private Queue<BattlefieldCharacter> GetOrCreatePool(CharacterKey key) {
        if(_pools.TryGetValue(key, out Queue<BattlefieldCharacter> pool)) {
            return pool;
        }

        pool = new Queue<BattlefieldCharacter>();
        _pools.Add(key, pool);
        return pool;
    }

    private BattlefieldCharacter CreateInstance(CharacterKey key) {
        CharacterConfig config = _configRegistry.Get(key);
        BattlefieldCharacter character =_container.InstantiatePrefabForComponent<BattlefieldCharacter>(config.Prefab);
        character.Initialize(key);
        return character;
    }
}