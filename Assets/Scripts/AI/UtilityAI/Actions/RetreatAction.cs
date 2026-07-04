public class RetreatAction : UtilityAction {
    private const float LOW_HEALTH_RETREAT_SCORE = 200f;

    public override AIActionType ActionType => AIActionType.Retreat;

    public override float CalculateScore(AIContext context) {
        if (context.CurrentTarget == null)
            return 0f;

        ICharacterRetreatConfig config =
            context.Self.Config as ICharacterRetreatConfig;

        if (config == null || config.RetreatHPThreshold <= 0f) return 0f;

        return context.CurrentHpPercent <= config.RetreatHPThreshold ?
            LOW_HEALTH_RETREAT_SCORE : 0f;
    }
}