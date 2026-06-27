public class AttackAction : UtilityAction {
    public override AIActionType ActionType => AIActionType.Attack;

    public override float CalculateScore(AIContext context) {
        if(context.CurrentTarget == null) {
            return 0f;
        }

        float sqrAttackRange = context.Self.Config.AttackRange * context.Self.Config.AttackRange;

        if (context.SqrDistanceToTarget > sqrAttackRange) {
            return 0f;
        }

        return 120f;
    }
}
