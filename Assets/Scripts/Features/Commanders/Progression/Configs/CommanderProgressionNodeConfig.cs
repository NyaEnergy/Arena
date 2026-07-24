using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Commanders/Progressions/Node Config",
                 fileName = "CommanderProgressionNodeConfig")]
public sealed class CommanderProgressionNodeConfig : ScriptableObject {
    [Header("Identity")]
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField, TextArea] private string _description;
    [SerializeField] private CommanderProgressionNodeType _nodeType;

    [Header("Unlock")]
    [SerializeField] private CommanderQuestConfig _quest;
    [SerializeField] private List<CommanderProgressionNodeConfig> _prerequisites = new();

    [Header("Upgrade Effect")]
    [SerializeField] private CommanderUpgradeEffectType _upgradeEffectType;
    [SerializeField, Min(0.01f)] private float _upgradeEffectMultiplier = 1f;

    [Header("Skill Effect")]
    [SerializeField] private CommanderSkillEffectType _skillEffectType;
    [SerializeField, Min(0f)] private float _skillEffectPower = 1f;
    [SerializeField, Min(0f)] private float _skillEffectDuration;
    [SerializeField, Min(0.1f)] private float _skillCooldown = 1f;
    [SerializeField, Min(0f)] private float _skillEffectRadius;
    [SerializeField, Min(1)] private int _skillDeploymentCount = 1;

    [Header("Upward Layout")]
    [SerializeField, Min(0)] private int _tier;
    [SerializeField] private int _column;

    public string Id => string.IsNullOrWhiteSpace(_id) ?
                        string.Empty : _id.Trim();

    public string DisplayName => string.IsNullOrWhiteSpace(_displayName) ?
                                 name : _displayName.Trim();

    public string Description => _description;
    public CommanderProgressionNodeType NodeType => _nodeType;
    public CommanderQuestConfig Quest => _quest;
    public IReadOnlyList<CommanderProgressionNodeConfig> Prerequisites => _prerequisites;
    public CommanderUpgradeEffectType UpgradeEffectType => _upgradeEffectType;
    public float UpgradeEffectMultiplier =>
        Mathf.Max(0.01f, _upgradeEffectMultiplier);
    public CommanderSkillEffectType SkillEffectType => _skillEffectType;
    public float SkillEffectPower => Mathf.Max(0f, _skillEffectPower);
    public float SkillEffectDuration => Mathf.Max(0f, _skillEffectDuration);
    public float SkillCooldown => Mathf.Max(0.1f, _skillCooldown);
    public float SkillEffectRadius => Mathf.Max(0f, _skillEffectRadius);
    public int SkillDeploymentCount => Mathf.Max(1, _skillDeploymentCount);

    public int Tier => Mathf.Max(0, _tier);
    public int Column => _column;

    public bool IsValid {
        get {
            if (string.IsNullOrWhiteSpace(Id) ||
                _quest == null ||
                !_quest.IsValid ||
                _prerequisites == null ||
                _tier < 0 ||
                !IsEffectValid()) return false;

            HashSet<string> prerequisiteIds =
                new(StringComparer.Ordinal);

            for (int i = 0; i < _prerequisites.Count; ++i) {
                CommanderProgressionNodeConfig prerequisite = _prerequisites[i];

                if (prerequisite == null ||
                    string.IsNullOrWhiteSpace(prerequisite.Id) ||
                    string.Equals(prerequisite.Id,
                                  Id, StringComparison.Ordinal) ||
                    !prerequisiteIds.Add(prerequisite.Id)) return false;
            }

            return true;
        }
    }

    private bool IsEffectValid() {
        return _nodeType switch {
            CommanderProgressionNodeType.Upgrade =>
                IsUpgradeEffectValid(),

            CommanderProgressionNodeType.Skill =>
                IsSkillEffectValid(),

            _ => false
        };
    }

    private bool IsUpgradeEffectValid() {
        return _upgradeEffectType != CommanderUpgradeEffectType.None &&
               _skillEffectType == CommanderSkillEffectType.None &&
               IsFinitePositive(_upgradeEffectMultiplier);
    }

    private bool IsSkillEffectValid() {
        if (_upgradeEffectType != CommanderUpgradeEffectType.None ||
            _skillEffectType == CommanderSkillEffectType.None ||
            !IsFiniteNonNegative(_skillEffectPower) ||
            !IsFiniteNonNegative(_skillEffectDuration) ||
            !IsFinitePositive(_skillCooldown) ||
            !IsFiniteNonNegative(_skillEffectRadius) ||
            _skillDeploymentCount < 1) {
            return false;
        }

        return _skillEffectType switch {
            CommanderSkillEffectType.DamageTakenMultiplier =>
                _skillEffectPower > 0f &&
                _skillEffectPower <= 1f &&
                _skillEffectDuration > 0f,

            CommanderSkillEffectType.AreaSlow =>
                _skillEffectPower < 1f &&
                _skillEffectDuration > 0f &&
                _skillEffectRadius > 0f,

            CommanderSkillEffectType.VanguardAssault => true,

            CommanderSkillEffectType.SummonerNetwork =>
                _skillDeploymentCount >= 2 &&
                _skillEffectRadius > 0f,

            _ => false
        };
    }

    private static bool IsFinitePositive(float value) {
        return value > 0f &&
               !float.IsNaN(value) &&
               !float.IsInfinity(value);
    }

    private static bool IsFiniteNonNegative(float value) {
        return value >= 0f &&
               !float.IsNaN(value) &&
               !float.IsInfinity(value);
    }
}