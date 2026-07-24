using UnityEngine;
using UnityEngine.UI;

public sealed class CommanderProgressionPanelView : MonoBehaviour {
    [SerializeField] private GameObject _panel;
    [SerializeField] private Button _openButton;
    [SerializeField] private Button _closeButton;
    [SerializeField] private CommanderProgressionTreeView _allyTree;
    [SerializeField] private CommanderProgressionTreeView _enemyTree;

    public Button OpenButton => _openButton;
    public Button CloseButton => _closeButton;
    public CommanderProgressionTreeView AllyTree => _allyTree;
    public CommanderProgressionTreeView EnemyTree => _enemyTree;

    public void SetOpen(bool isOpen) {
        if (_panel != null) {
            _panel.SetActive(isOpen);
        }

        if (_openButton != null) {
            _openButton.gameObject.SetActive(!isOpen);
        }
    }
}
