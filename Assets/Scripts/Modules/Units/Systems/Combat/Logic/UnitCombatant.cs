namespace ConveyorWars.Units.Combat {
    public sealed class UnitCombatant : IHealthReadOnly {
        private readonly UnitEntity _owner;
        private readonly UnitCombatSettings _settings;
        private readonly UnitCombatRuntime _runtime;

        public int CurrentHealth => _runtime.CurrentHealth;
        public int MaxHealth => _settings.MaxHealth;
        public bool IsAlive => _runtime.IsAlive;
        public UnitEntity CurrentTarget => _runtime.CurrentTarget;
        public bool HasTarget => _runtime.HasTarget;
        public int Damage => _settings.Damage;
        public float AttackRange => _settings.AttackRange;
        public float AttackInterval => _settings.AttackInterval;
        public bool IsAttackReady => _runtime.IsAttackReady;

        public UnitCombatant(
            UnitEntity owner,
            UnitCombatSettings settings,
            UnitCombatRuntime runtime) {
            _owner = owner;
            _settings = settings;
            _runtime = runtime;
        }

        public void Tick(float deltaTime) {
            _runtime.Tick(deltaTime);
        }

        public bool TrySetTarget(UnitEntity target) {
            if (target == null ||
                target == _owner ||
                target.Side == _owner.Side ||
                !IsAlive) {
                return false;
            }

            return _runtime.TrySetTarget(target);
        }

        public void ClearTarget() {
            _runtime.ClearTarget();
        }

        public bool TryTakeDamage(
            int damage,
            out int appliedDamage) {
            return _runtime.TryTakeDamage(
                damage,
                out appliedDamage);
        }

        public bool TryStartAttackCooldown() {
            return _runtime.TryStartAttackCooldown(
                _settings.AttackInterval);
        }

        public bool TryAttack(UnitCombatant target, out int appliedDamage) {
            appliedDamage = 0;

            if (target == null ||
                target == this ||
                !IsAlive ||
                !target.IsAlive ||
                !IsAttackReady ||
                CurrentTarget != target._owner)
                return false;

            if (!target.TryTakeDamage(Damage,
                    out appliedDamage))
                return false;

            if (!TryStartAttackCooldown()) return false;

            if (!target.IsAlive) ClearTarget();

            return true;
        }
    }
}