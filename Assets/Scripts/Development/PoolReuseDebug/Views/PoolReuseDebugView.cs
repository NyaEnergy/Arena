using TMPro;
using UnityEngine;

public class PoolReuseDebugView : MonoBehaviour {
    [SerializeField] private TMP_Text _statusText;

    public void SetText(string text) {
        if (_statusText == null) return;
        _statusText.text = text;
    }
}
