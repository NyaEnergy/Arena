public class IdleAction : UtilityAction {
    public override AIActionType ActionType => AIActionType.Idle;

    public override float CalculateScore(AIContext context) {
        if(context.CurrentTarget == null) {
            return 100f;
        }
        return 0f;
    }
}
