using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterPool {
    private readonly DiContainer _container;
    private readonly CharacterConfigRegistry _configRegistry;

    private readonly Dictionary<CharacterKey, Queue<BattlefieldCharacter>> _pools = new();

    public CharacterPool(DiContainer container,
                         CharacterConfigRegistry configRegistry) {
        _container = container;
        _configRegistry = configRegistry;
    }

    public void Warmup(CharacterKey key,
                       int preloadCount) {
        if (_pools.ContainsKey(key)) return;

        Queue<BattlefieldCharacter> pool = new();

        _pools.Add(key, pool);

        for (int i = 0; i < preloadCount; i++) {
            BattlefieldCharacter character = CreateInstance(key);
            Return(key, character);
        }
    }

    public BattlefieldCharacter Get(CharacterKey key,
                                    Vector3 position) {
        if (!_pools.ContainsKey(key))
            Warmup(key, 4);

        BattlefieldCharacter character =
            _pools[key].Count > 0 ?
            _pools[key].Dequeue() :
            CreateInstance(key);

        character.transform.position = position;
        character.OnSpawned();
        return character;
    }

    public void Return(CharacterKey key,
                       BattlefieldCharacter character) {
        character.OnDespawned();
        _pools[key].Enqueue(character);
    }

    private BattlefieldCharacter CreateInstance(CharacterKey key) {
        CharacterConfig config = _configRegistry.Get(key);
        BattlefieldCharacter character =_container.InstantiatePrefabForComponent<BattlefieldCharacter>(config.Prefab);
        return character;
    }
}