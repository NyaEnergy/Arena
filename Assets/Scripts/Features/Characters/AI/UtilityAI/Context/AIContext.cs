public class AIContext {
    public CharacterBrain Self;
    public CharacterBrain CurrentTarget;

    public float SqrDistanceToTarget;
    public float CurrentHpPercent;

    public void Reset() {
        Self = null;
        CurrentTarget = null;

        SqrDistanceToTarget = float.MaxValue;
        CurrentHpPercent = 0f;
    }
}
