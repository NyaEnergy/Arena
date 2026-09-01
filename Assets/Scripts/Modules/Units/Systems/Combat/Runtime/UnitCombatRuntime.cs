namespace ConveyorWars.Units.Combat {
    public sealed class UnitCombatRuntime {
        public int CurrentHealth { get; private set; }
        public float AttackCooldownRemaining { get; private set; }
        public UnitEntity CurrentTarget { get; private set; }

        public bool IsAlive => CurrentHealth > 0;
        public bool HasTarget => CurrentTarget != null;

        public bool IsAttackReady =>
            IsAlive &&
            AttackCooldownRemaining <= 0f;

        public UnitCombatRuntime(int maxHealth) {
            CurrentHealth = maxHealth;
        }

        public void Tick(float deltaTime) {
            if (deltaTime <= 0f ||
                AttackCooldownRemaining <= 0f) {
                return;
            }

            AttackCooldownRemaining -= deltaTime;

            if (AttackCooldownRemaining < 0f) {
                AttackCooldownRemaining = 0f;
            }
        }

        public bool TrySetTarget(
            UnitEntity target) {
            if (!IsAlive || target == null) {
                return false;
            }

            CurrentTarget = target;
            return true;
        }

        public void ClearTarget() {
            CurrentTarget = null;
        }

        public bool TryStartAttackCooldown(
            float attackInterval) {
            if (!IsAttackReady ||
                attackInterval <= 0f) {
                return false;
            }

            AttackCooldownRemaining =
                attackInterval;

            return true;
        }

        public bool TryTakeDamage(
            int damage,
            out int appliedDamage) {
            appliedDamage = 0;

            if (!IsAlive ||
                damage <= 0) {
                return false;
            }

            appliedDamage =
                damage > CurrentHealth
                    ? CurrentHealth
                    : damage;

            CurrentHealth -= appliedDamage;

            if (!IsAlive) {
                ClearTarget();
            }

            return true;
        }
    }
}