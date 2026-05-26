using UnityEngine;

public class CharacterFactory {
    private readonly CharacterPool _characterPool;

    public CharacterFactory(CharacterPool characterPool) {
        _characterPool = characterPool;
    }

    public BattlefieldCharacter Spawn(CharacterKey key, Vector3 position) {
        BattlefieldCharacter character = _characterPool.Get(key, position);
        character.Initialize(key);
        return character;
    }
}
