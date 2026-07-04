using UnityEngine;

public class CombatComponent {
    private readonly CharacterBrain _owner;
    private readonly ICharacterAttackConfig _config;
    private readonly TargetComponent _targetComponent;

    private float _lastAttackTime;

    public bool CanAttack {
        get {
            CharacterBrain target = _targetComponent.CurrentTarget.CurrentValue;

            if (target == null ||
                target.Runtime.IsDead.CurrentValue) return false;

            float sqrDistance = Vector3.SqrMagnitude(_owner.View.transform.position -
                                                     target.View.transform.position);

            Range attackDistanceRange = _config.AttackDistanceRange;

            float sqrMinimumAttackDistance = attackDistanceRange.Min *
                                             attackDistanceRange.Min;

            float sqrMaximumAttackDistance = attackDistanceRange.Max *
                                             attackDistanceRange.Max;

            if (sqrDistance < sqrMinimumAttackDistance) return false;
            if (sqrDistance > sqrMaximumAttackDistance) return false;

            return Time.time >= _lastAttackTime +
                                _config.AttackCooldown;
        }
    }

    public CombatComponent(CharacterBrain owner,
                           ICharacterAttackConfig config,
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
        if (!CanAttack) return false;

        CharacterBrain target = _targetComponent
                                .CurrentTarget
                                .CurrentValue;

        target.HealthComponent.ApplyDamage(_config.Damage);
        _lastAttackTime = Time.time;
        return true;
    }
}