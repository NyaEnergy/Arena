public class DeadState : CharacterState {
    public override CharacterStateType StateType => CharacterStateType.Dead;
    public DeadState(CharacterBrain brain) : base(brain) { }
    public override void Enter() {
        _brain.MovementComponent.Stop();
        _brain.View.Agent.enabled = false;
        _brain.View.Animator.SetTrigger("Death");
        UnityEngine.Debug.Log($"{_brain.View.name} is Dead");
    }
}
