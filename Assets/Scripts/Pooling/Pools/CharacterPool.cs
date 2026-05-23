using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class CharacterPool {
    private readonly DiContainer _container;
    private readonly Dictionary<CharacterType, Queue<BattlefieldCharacter>> _pools = new();
    private readonly Dictionary<CharacterType, BattlefieldCharacter> _prefabs = new();

    public CharacterPool(DiContainer container) {
        _container = container;
    }

    public void Initialize(CharacterType characterType, BattlefieldCharacter prefab, int preloadCount) {
        if (_pools.ContainsKey(characterType)) return;
        _prefabs.Add(characterType, prefab);
        _pools.Add(characterType, new Queue<BattlefieldCharacter>());
        Queue<BattlefieldCharacter> pool = _pools[characterType];
        for(int i = 0; i < preloadCount; ++i) {
            BattlefieldCharacter character = CreateInstance(characterType);
            Return(characterType, character);
        }
    }

    public BattlefieldCharacter Get(CharacterType characterType, Vector3 position) {
        Queue<BattlefieldCharacter> pool = _pools[characterType];
        BattlefieldCharacter character = pool.Count > 0 ? pool.Dequeue() : CreateInstance(characterType);
        character.transform.position = position;
        character.OnSpawned();
        return character;
    }

    public void Return(CharacterType characterType, BattlefieldCharacter character) {
        character.OnDespawned();
        _pools[characterType].Enqueue(character);
    }

    private BattlefieldCharacter CreateInstance(CharacterType characterType) {
        BattlefieldCharacter prefab = _prefabs[characterType];
        return _container.InstantiatePrefabForComponent<BattlefieldCharacter>(prefab);
    }
}
