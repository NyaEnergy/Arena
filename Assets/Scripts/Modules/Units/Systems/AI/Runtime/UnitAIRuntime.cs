namespace ConveyorWars.Units.AI {
    public sealed class UnitAIRuntime {
        public float DecisionCooldownRemaining { get; private set; }
        public bool IsDecisionReady => DecisionCooldownRemaining <= 0f;

        public void Tick(float deltaTime) {
            if (deltaTime <= 0f || DecisionCooldownRemaining <= 0f) return;

            DecisionCooldownRemaining -= deltaTime;

            if (DecisionCooldownRemaining < 0f) {
                DecisionCooldownRemaining = 0f;
            }
        }

        public bool TryStartDecisionCooldown(float interval) {
            if (!IsDecisionReady || interval <= 0f) return false;

            DecisionCooldownRemaining = interval;
            return true;
        }
    }
}