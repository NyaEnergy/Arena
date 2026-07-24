using TMPro;
using UnityEngine;
using UnityEngine.UI;

public sealed class CommanderSkillButtonView : MonoBehaviour {
    [SerializeField] private Button _button;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _title;
    [SerializeField] private Image _cooldownFill;
    [SerializeField] private TMP_Text _status;
    [SerializeField] private GameObject _selection;

    public Button Button => _button;

    public void Render(CommanderProgressionRuntime skill,
                       bool isReady,
                       float cooldownRemaining,
                       bool isSelected) {

        bool hasSkill = skill != null;

        gameObject.SetActive(hasSkill);

        if (!hasSkill) return;

        bool isLocked = !skill.IsUnlocked;
        bool isCoolingDown = cooldownRemaining > 0f;

        if (_icon != null) {
            _icon.sprite = skill.Commander.Icon;
            _icon.enabled = _icon.sprite != null;
        }

        if (_title != null) {
            _title.text = $"{skill.Commander.DisplayName}\n" +
                          skill.Node.DisplayName;
        }

        if (_cooldownFill != null) {
            _cooldownFill.fillAmount = isLocked ? 1f :
                Mathf.Clamp01(
                    cooldownRemaining /
                    skill.Node.SkillCooldown);

            _cooldownFill.gameObject.SetActive(
                isLocked || isCoolingDown);
        }

        if (_status != null) {
            _status.text = isLocked ?
                           "Закрыто" : isCoolingDown ?
                           Mathf.CeilToInt(cooldownRemaining).ToString() :
                           string.Empty;

            _status.gameObject.SetActive(
                !string.IsNullOrEmpty(_status.text));
        }

        if (_selection != null) {
            _selection.SetActive(isSelected);
        }

        if (_button != null) {
            _button.interactable = isReady;
        }
    }
}