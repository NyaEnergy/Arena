using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CommanderSelectionOptionView : MonoBehaviour {
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _status;
    [SerializeField] private GameObject _selection;

    public Button Button => _button;
    public CommanderConfig Commander { get; private set; }

    public void Render(CommanderConfig commander,
                       bool isSelected,
                       bool isInteractable) {

        Commander = commander;
        gameObject.SetActive(commander != null);

        if (commander == null) return;

        if (_icon != null) {
            _icon.sprite = commander.Icon;
            _icon.enabled = _icon.sprite != null;
        }

        if (_status != null) {
            _status.text = commander.DisplayName;
        }

        if (_selection != null) {
            _selection.SetActive(isSelected);
        }

        if (_button != null) {
            _button.interactable = isInteractable;
        }
    }

    public void Hide() {
        Commander = null;
        gameObject.SetActive(false);
    }
}
