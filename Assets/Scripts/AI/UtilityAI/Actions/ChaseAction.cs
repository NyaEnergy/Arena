public class ChaseAction : UtilityAction {
    public override AIActionType ActionType => AIActionType.Chase;

    public override float CalculateScore(AIContext context) {
        if(context.CurrentTarget == null) {
            return 0f;
        }
        if(context.DistanceToTarget > context.Self.Config.AttackRange * context.Self.Config.AttackRange) {
            return 80f;
        }
        return 0f;
    }
}
