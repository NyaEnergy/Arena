using TMPro;
using UnityEngine;

public sealed class CommanderSkillHudView : MonoBehaviour {
    [SerializeField] private CommanderSkillButtonView _allySkillButton;
    [SerializeField] private CommanderSkillButtonView _enemySkillButton;
    [SerializeField] private TMP_Text _targetingText;

    public CommanderSkillButtonView AllySkillButton => _allySkillButton;
    public CommanderSkillButtonView EnemySkillButton => _enemySkillButton;

    public void SetTargeting(CommanderProgressionRuntime skill) {
        if (_targetingText == null) return;

        bool isTargeting = skill != null;

        _targetingText.text = isTargeting ?
            $"Выберите область: {skill.Node.DisplayName}" :
            string.Empty;

        _targetingText.gameObject.SetActive(isTargeting);
    }
}