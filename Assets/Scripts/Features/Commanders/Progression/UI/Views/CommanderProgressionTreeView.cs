using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CommanderProgressionTreeView : MonoBehaviour {
    [SerializeField] private Image _commanderIcon;
    [SerializeField] private TMP_Text _commanderName;
    [SerializeField] private TMP_Text _treeName;
    [SerializeField] private List<CommanderProgressionNodeView> _nodeViews = new();

    public IReadOnlyList<CommanderProgressionNodeView> NodeViews => _nodeViews;

    public void RenderHeader(CommanderConfig commander) {
        bool hasCommander = commander != null &&
                            commander.ProgressionTree != null;

        gameObject.SetActive(hasCommander);

        if (!hasCommander) return;

        if (_commanderIcon != null) {
            _commanderIcon.sprite = commander.Icon;
            _commanderIcon.enabled = commander.Icon != null;
        }

        if (_commanderName != null) {
            _commanderName.text = commander.DisplayName;
        }

        if (_treeName != null) {
            _treeName.text = commander.ProgressionTree.DisplayName;
        }
    }
}
