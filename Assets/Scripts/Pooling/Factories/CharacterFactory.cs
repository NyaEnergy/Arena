using UnityEngine;

public class CharacterFactory {
    private readonly CharacterPool _characterPool;

    public CharacterFactory(CharacterPool characterPool) {
        _characterPool = characterPool;
    }

    public BattlefieldCharacter Spawn(CharacterType characterType, Vector3 position) {
        return _characterPool.Get(characterType, position);
    }
}
