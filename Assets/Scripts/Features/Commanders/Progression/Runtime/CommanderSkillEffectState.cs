using System.Collections.Generic;
using UnityEngine;

public sealed class CommanderSkillEffectState {
    private readonly List<CommanderSkillEffectRuntime> _activeEffects = new();

    public IReadOnlyList<CommanderSkillEffectRuntime> ActiveEffects =>
        _activeEffects;

    public bool TryActivate(CommanderProgressionRuntime skill,
                            Vector3 position) {
        if (skill == null ||
            skill.Node.NodeType != CommanderProgressionNodeType.Skill ||
            skill.Node.SkillEffectDuration <= 0f ||
            IsActive(skill)) {
            return false;
        }

        _activeEffects.Add(new CommanderSkillEffectRuntime(
            skill,
            position));

        return true;
    }

    public bool IsActive(CommanderProgressionRuntime skill) {
        for (int i = 0; i < _activeEffects.Count; ++i) {
            if (_activeEffects[i].Matches(skill)) return true;
        }

        return false;
    }

    public float GetDamageTakenMultiplier(TeamType teamType) {
        float multiplier = 1f;

        for (int i = 0; i < _activeEffects.Count; ++i) {
            CommanderSkillEffectRuntime effect = _activeEffects[i];

            if (effect.TeamType == teamType &&
                effect.EffectType ==
                    CommanderSkillEffectType.DamageTakenMultiplier) {
                multiplier *= effect.Power;
            }
        }

        return Mathf.Max(0f, multiplier);
    }

    internal void Tick(float deltaTime) {
        for (int i = _activeEffects.Count - 1; i >= 0; --i) {
            if (!_activeEffects[i].Tick(deltaTime)) {
                _activeEffects.RemoveAt(i);
            }
        }
    }
}