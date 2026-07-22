using UnityEngine;

public sealed class CommanderSkillEffectRuntime {
    public CommanderProgressionRuntime Skill { get; }
    public Vector3 Position { get; }
    public float RemainingTime { get; private set; }

    public TeamType TeamType => Skill.Commander.TeamType;
    public CommanderSkillEffectType EffectType =>
        Skill.Node.SkillEffectType;

    public float Power => Skill.Node.SkillEffectPower;
    public float Radius => Skill.Node.SkillEffectRadius;

    public CommanderSkillEffectRuntime(
        CommanderProgressionRuntime skill,
        Vector3 position) {
        Skill = skill;
        Position = position;
        RemainingTime = skill.Node.SkillEffectDuration;
    }

    public bool Matches(CommanderProgressionRuntime skill) {
        return skill != null &&
               Skill.Commander == skill.Commander &&
               Skill.Node == skill.Node;
    }

    internal bool Tick(float deltaTime) {
        RemainingTime = Mathf.Max(
            0f,
            RemainingTime - Mathf.Max(0f, deltaTime));

        return RemainingTime > 0f;
    }
}
