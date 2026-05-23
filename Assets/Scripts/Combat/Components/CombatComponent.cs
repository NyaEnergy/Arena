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
            float distance = Vector3.SqrMagnitude(_owner.View.transform.position - target.View.transform.position);
            if (distance > _config.AttackRange * _config.AttackRange) return false;
            if (Time.time < _lastAttackTime + _config.AttackCooldown) return false;
            return true;
        }
    }

    public CombatComponent(CharacterBrain owner, CharacterConfig config, TargetComponent targetComponent) {
        _owner = owner;
        _config = config;
        _targetComponent = targetComponent;

        _lastAttackTime = -999f;
    }

    public void Attack() {
        CharacterBrain target = _targetComponent.CurrentTarget.CurrentValue;
        if (target == null) return;
        target.HealthComponent.ApplyDamage(_config.Damage);
        _lastAttackTime = Time.time;
        Debug.Log($"{target.View.name} HP: {target.HealthComponent.CurrentHP}");
    }
}
