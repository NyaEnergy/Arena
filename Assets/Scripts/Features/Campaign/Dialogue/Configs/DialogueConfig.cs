using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Campaign/Dialogue Config",
                 fileName = "DialogueConfig")]
public sealed class DialogueConfig : ScriptableObject {
    [SerializeField] private List<DialogueLine> _lines = new();

    public IReadOnlyList<DialogueLine> Lines => _lines;

    public bool IsValid {
        get {
            if (_lines == null ||
                _lines.Count == 0) return false;

            for (int i = 0; i < _lines.Count; ++i) {
                if (_lines[i] == null ||
                    !_lines[i].IsValid) return false;
            }

            return true;
        }
    }
}
