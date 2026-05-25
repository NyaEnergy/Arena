using UnityEngine;

public class RetreatState : CharacterState {
    public override CharacterStateType StateType => CharacterStateType.Move;

    public RetreatState(CharacterBrain brain) : base(brain) { }

    public override void Tick() {
        CharacterBrain target = _brain.TargetComponent.CurrentTarget.CurrentValue;

        if (target == null) return;

        Vector3 distance = (_brain.View.transform.position - target.View.transform.position).normalized;
        Vector3 retreatPosition = _brain.View.transform.position + distance * 6f;
        _brain.MovementComponent.MoveTo(retreatPosition);
    }
}
