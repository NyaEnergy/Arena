public class ChaseAction : UtilityAction {
    private const float CHASE_SCORE = 80f;

    public override AIActionType ActionType => AIActionType.Chase;

    public override float CalculateScore(AIContext context) {
        if (context.CurrentTarget == null) return 0f;

        ICharacterAttackConfig config =
            context.Self.Config as ICharacterAttackConfig;

        if (context.CurrentTarget == null) return 0f;

        Range attackDistanceRange = config.AttackDistanceRange;
        float sqrMaximumAttackDistance = attackDistanceRange.Max *
                                         attackDistanceRange.Max;

        return context.SqrDistanceToTarget > sqrMaximumAttackDistance ?
            CHASE_SCORE : 0f;
    }
}