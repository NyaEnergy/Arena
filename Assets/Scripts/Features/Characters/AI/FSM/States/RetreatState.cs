using UnityEngine;

public class RetreatState : CharacterState {
    private const float MINIMUM_SQR_DIRECTION = 0.001f;

    public override CharacterStateType StateType => CharacterStateType.Retreat;

    public RetreatState(CharacterBrain brain) : base(brain) { }

    public override void Tick() {
        CharacterBrain threat = _brain.TargetComponent.CurrentTarget.CurrentValue;

        ICharacterRetreatConfig config =
            _brain.Config as ICharacterRetreatConfig;

        if (threat == null || config == null) return;

        Vector3 characterPosition = _brain.View.transform.position;
        Vector3 threatPosition = threat.View.transform.position;
        Vector3 retreatDirection = characterPosition - threatPosition;
        retreatDirection.y = 0f;

        float retreatDistance = config.RetreatDistance;
        float sqrRetreatDistance = retreatDistance *
                                   retreatDistance;

        if (retreatDirection.sqrMagnitude >= sqrRetreatDistance) {
            _brain.MovementComponent.Stop();
            return;
        }

        if (retreatDirection.sqrMagnitude < MINIMUM_SQR_DIRECTION) {
            retreatDirection = -threat.View.transform.forward;
            retreatDirection.y = 0f;
        }

        retreatDirection.Normalize();

        Vector3 retreatPosition = threatPosition + retreatDirection *
                                                   retreatDistance;

        _brain.MovementComponent.MoveToPosition(retreatPosition);
    }
}