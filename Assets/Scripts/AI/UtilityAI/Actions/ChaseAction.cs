public class ChaseAction : UtilityAction {
    public override AIActionType ActionType => AIActionType.Chase;

    public override float CalculateScore(AIContext context) {
        if(context.CurrentTarget == null) {
            return 0f;
        }

        float sqrAttackRange = context.Self.Config.AttackRange * context.Self.Config.AttackRange;

        if (context.SqrDistanceToTarget <= sqrAttackRange) {
            return 0f;
        }

        return 80f;
    }
}
