public class RetreatAction : UtilityAction {
    public override AIActionType ActionType => AIActionType.Retreat;

    public override float CalculateScore(AIContext context) {
        if(context.CurrentHpPercent > 0.25f) {
            return 0f;
        }
        return 200f;
    }
}
