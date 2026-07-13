public class AttackAction : UtilityAction {
    private const float ATTACK_SCORE = 120f;

    public override AIActionType ActionType => AIActionType.Attack;

    public override float CalculateScore(AIContext context) {
        if (context.CurrentTarget == null) return 0f;

        ICharacterAttackConfig config =
            context.Self.Config as ICharacterAttackConfig;

        if (config == null) return 0f;

        Range attackDistanceRange = config.AttackDistanceRange;

        Range sqrAttackDistanceRange = new(
            attackDistanceRange.Min * attackDistanceRange.Min,
            attackDistanceRange.Max * attackDistanceRange.Max);

        return context.SqrDistanceToTarget >= sqrAttackDistanceRange.Min &&
               context.SqrDistanceToTarget <= sqrAttackDistanceRange.Max ?
               ATTACK_SCORE : 0f;
    }
}