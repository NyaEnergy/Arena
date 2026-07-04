using System.Collections.Generic;

public class CharacterPrefabRegistry {
    private readonly Dictionary<CharacterType, CharacterView> _prefabs = new();

    public CharacterPrefabRegistry(List<CharacterView> prefabs) {
        for(int i = 0; i < prefabs.Count; ++i) {
            CharacterView prefab = prefabs[i];

            if (prefab == null) continue;

            _prefabs[prefab.CharacterType] = prefab;
        }
    }

    public CharacterView Get(CharacterType characterType) {
        _prefabs.TryGetValue(characterType, out CharacterView prefab);
        return prefab;
    }

    public CharacterView Get(CharacterKey key) {
        return Get(key.CharacterType);
    }
}
