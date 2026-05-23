public class IdleState : CharacterState {
    public override CharacterStateType StateType => CharacterStateType.Idle;
    public IdleState(CharacterBrain brain) : base(brain) { }
    public override void Enter() {
        _brain.MovementComponent.Stop();
    }
}
