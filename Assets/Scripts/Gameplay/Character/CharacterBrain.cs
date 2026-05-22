public class CharacterBrain {
    public readonly CharacterRuntime Runtime;
    public readonly HealthComponent Health;
    public readonly MovementComponent Movement;
    public readonly CombatComponent Combat;
    public readonly TargetComponent Target;

    public CharacterBrain(
        CharacterRuntime runtime,
        HealthComponent health,
        MovementComponent movement,
        CombatComponent combat,
        TargetComponent target) {
        Runtime = runtime;
        Health = health;
        Movement = movement;
        Combat = combat;
        Target = target;
    }
}
