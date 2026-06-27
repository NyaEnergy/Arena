using UnityEngine;

public class CombatComponent {
    private readonly CharacterBrain _owner;
    private readonly CharacterConfig _config;
    private readonly TargetComponent _targetComponent;

    private float _lastAttackTime;

    public bool IsCanAttack {
        get {
            CharacterBrain target = _targetComponent.CurrentTarget.CurrentValue;
            if (target == null) return false;
            if (target.Runtime.IsDead.CurrentValue) return false;
            float sqrDistance = Vector3.SqrMagnitude(_owner.View.transform.position - target.View.transform.position);
            float sqrAttackRange = _config.AttackRange * _config.AttackRange;
            if (sqrDistance > sqrAttackRange) return false;
            if (Time.time < _lastAttackTime + _config.AttackCooldown) return false;
            return true;
        }
    }

    public CombatComponent(CharacterBrain owner,
                           CharacterConfig config,
                           TargetComponent targetComponent) {
        _owner = owner;
        _config = config;
        _targetComponent = targetComponent;

        Reset();
    }

    public void Reset() {
        _lastAttackTime = float.NegativeInfinity;
    }

    public bool TryAttack() {
        if (!IsCanAttack) return false;
        CharacterBrain target = _targetComponent.CurrentTarget.CurrentValue;
        target.HealthComponent.ApplyDamage(_config.Damage);
        _lastAttackTime = Time.time;
        return true;
    }
}
