using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CampaignSelectionView : MonoBehaviour {
    [SerializeField] private GameObject _commanderSelection;
    [SerializeField]
    private CampaignArcOptionView[] _arcOptions =
        Array.Empty<CampaignArcOptionView>();
    [SerializeField]
    private CommanderSelectionOptionView[] _alliedCommanderOptions =
        Array.Empty<CommanderSelectionOptionView>();
    [SerializeField]
    private CommanderSelectionOptionView[] _enemyCommanderOptions =
        Array.Empty<CommanderSelectionOptionView>();
    [SerializeField] private Button _confirmButton;
    [SerializeField] private TMP_Text _confirmLabel;
    [SerializeField] private TMP_Text _status;

    public GameObject CommanderSelection => _commanderSelection;
    public IReadOnlyList<CampaignArcOptionView> ArcOptions => _arcOptions;
    public IReadOnlyList<CommanderSelectionOptionView> AlliedCommanderOptions =>
        _alliedCommanderOptions;
    public IReadOnlyList<CommanderSelectionOptionView> EnemyCommanderOptions =>
        _enemyCommanderOptions;
    public Button ConfirmButton => _confirmButton;

    public void SetCommanderSelectionVisible(bool isVisible) {
        if (_commanderSelection != null) {
            _commanderSelection.SetActive(isVisible);
        }
    }

    public void RenderConfirm(string label,
                              bool isInteractable) {
        if (_confirmLabel != null) {
            _confirmLabel.text = label;
        }

        if (_confirmButton != null) {
            _confirmButton.interactable = isInteractable;
        }
    }

    public void RenderStatus(string status,
                             Color color) {
        if (_status == null) return;

        _status.text = status;
        _status.color = color;
    }
}
