using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Presentation/Primitive Character Presentation Config",
    fileName = "PrimitiveCharacterPresentationConfig")]
public class PrimitiveCharacterPresentationConfig : ScriptableObject {
    [SerializeField] private Material _allyMaterial;
    [SerializeField] private Material _enemyMaterial;

    [SerializeField] private List<PrimitiveCharacterPresentationEntry> _entries = new();

    public bool TryGetEntry(CharacterType characterType,
                            out PrimitiveCharacterPresentationEntry entry) {
        for(int i = 0; i < _entries.Count; ++i) {
            PrimitiveCharacterPresentationEntry currentEntry = _entries[i];
            if (currentEntry.CharacterType != characterType) continue;
            entry = currentEntry;
            return true;
        }
        entry = null;
        return false;
    }

    public bool TryGetMaterial(TeamType teamType,
                                out Material material) {
        switch (teamType) {
            case TeamType.Ally:
                material = _allyMaterial;
                break;
            case TeamType.Enemy:
                material = _enemyMaterial;
                break;
            default:
                material = null;
                break;
        }
        return material != null;
    }
}
