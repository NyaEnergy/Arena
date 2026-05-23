public class MoveState : CharacterState {
    public override CharacterStateType StateType => CharacterStateType.Move;
    public MoveState(CharacterBrain brain) : base(brain) { }
    public override void Tick() {
        CharacterBrain target = _brain.TargetComponent.CurrentTarget.CurrentValue;
        if (target == null) return;
        _brain.MovementComponent.MoveTo(target.View.transform.position);
    }
}
