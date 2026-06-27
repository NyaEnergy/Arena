public class RetreatAction : UtilityAction {
    private const float RETREAT_HP_THRESHOLD = 0.25f;

    public override AIActionType ActionType => AIActionType.Retreat;

    public override float CalculateScore(AIContext context) {
        if (context.CurrentTarget == null ||
            context.CurrentHpPercent > RETREAT_HP_THRESHOLD) {
            return 0f;
        }
        return 200f;
    }
}
