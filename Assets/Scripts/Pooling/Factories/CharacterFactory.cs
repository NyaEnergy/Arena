using UnityEngine;

public class CharacterFactory {
    private readonly CharacterPool _characterPool;

    public CharacterFactory(CharacterPool characterPool) {
        _characterPool = characterPool;
    }

    public CharacterView Spawn(CharacterKey key, Vector3 position) {
        CharacterView character = _characterPool.Get(key, position);

        if (character == null) return null;

        character.EnterBattlefield();
        return character;
    }
}
