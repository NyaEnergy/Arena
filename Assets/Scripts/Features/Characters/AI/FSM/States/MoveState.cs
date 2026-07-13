public class MoveState : CharacterState {
    public override CharacterStateType StateType => CharacterStateType.Move;
    public MoveState(CharacterBrain brain) : base(brain) { }
    public override void Tick() {
        CharacterBrain target = _brain.TargetComponent.CurrentTarget.CurrentValue;

        ICharacterAttackConfig config =
            _brain.Config as ICharacterAttackConfig;

        if (target == null || config == null) return;

        _brain.MovementComponent
              .MoveToAttackRange(target.View.transform.position,
                                 config.AttackDistanceRange);
    }
}
