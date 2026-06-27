public class AttackState : CharacterState {
    public override CharacterStateType StateType => CharacterStateType.Attack;
    public AttackState(CharacterBrain brain) : base(brain) { }
    public override void Enter() {
        _brain.MovementComponent.Stop();
    }
    public override void Tick() {
        if (!_brain.CombatComponent.IsCanAttack) return;
        if(!_brain.CombatComponent.TryAttack()) return;
        _brain.View.PlayAttack();
    }
}
