using static UnityEngine.UI.GridLayoutGroup;

public class AttackState : CharacterState {
    public override CharacterStateType StateType => CharacterStateType.Attack;
    public AttackState(CharacterBrain brain) : base(brain) { }
    public override void Enter() {
        _brain.MovementComponent.Stop();
    }
    public override void Tick() {
        if (_brain.CombatComponent.IsCanAttack) {
            _brain.CombatComponent.Attack();
            _brain.View.Animator.SetTrigger("Attack");
        }
    }
}
