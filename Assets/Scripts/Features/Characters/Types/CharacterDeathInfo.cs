using UnityEngine;

public readonly struct CharacterDeathInfo {
    public readonly CharacterKey CharacterKey;
    public readonly Vector3 Position;

    public TeamType TeamType => CharacterKey.TeamType;
    public CharacterType CharacterType => CharacterKey.CharacterType;

    public CharacterDeathInfo(CharacterKey characterKey,
                              Vector3 position) {
        CharacterKey = characterKey;
        Position = position;
    }
}