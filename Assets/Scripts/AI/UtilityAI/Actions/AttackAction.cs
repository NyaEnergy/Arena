public class AttackAction : UtilityAction {
    public override AIActionType ActionType => AIActionType.Attack;

    public override float CalculateScore(AIContext context) {
        if(context.CurrentTarget == null) {
            return 0f;
        }
        if(context.DistanceToTarget <= context.Self.Config.AttackRange * context.Self.Config.AttackRange) {
            return 120f;
        }
        return 0f;
    }
}
