public class AttackState : CharacterState {
    public override CharacterStateType StateType => CharacterStateType.Attack;

    public AttackState(CharacterBrain brain) : base(brain) { }

    public override void Enter() {
        _brain.MovementComponent.Stop();
    }

    public override void Tick() {
        CombatComponent combatComponent = _brain.CombatComponent;

        CharacterBrain target = _brain.TargetComponent
                                      .CurrentTarget
                                      .CurrentValue;

        if (combatComponent == null || target == null ||
            !combatComponent.TryAttack()) return;

        _brain.View.PlayAttack(target.View);

        target.View.PlayHit();
    }
}