using UnityEngine;

public class RetreatState : CharacterState {
    private const float RETREAT_DISTANCE = 6f;

    public override CharacterStateType StateType => CharacterStateType.Retreat;

    public RetreatState(CharacterBrain brain) : base(brain) { }

    public override void Tick() {
        CharacterBrain target = _brain.TargetComponent.CurrentTarget.CurrentValue;

        if (target == null) return;

        Vector3 retreatDistance = (_brain.View.transform.position - target.View.transform.position).normalized;
        Vector3 retreatPosition = _brain.View.transform.position + retreatDistance * RETREAT_DISTANCE;
        _brain.MovementComponent.MoveTo(retreatPosition);
    }
}
