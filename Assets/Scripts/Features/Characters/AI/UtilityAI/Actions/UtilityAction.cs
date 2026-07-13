public abstract class UtilityAction {
    public abstract AIActionType ActionType { get; }
    public abstract float CalculateScore(AIContext context);
}
