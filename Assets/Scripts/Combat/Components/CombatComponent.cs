using UnityEngine;

public class CombatComponent {
    private readonly CharacterBrain _owner;
    private readonly CharacterConfig _config;
    private readonly TargetComponent _targetComponent;

    private float _lastAttackTime;

    public CombatComponent(CharacterBrain owner, CharacterConfig config, TargetComponent targetComponent) {
        _owner = owner;
        _config = config;
        _targetComponent = targetComponent;

        _lastAttackTime = -999f;
    }

    public bool CanAttack() {
        CharacterBrain target = _targetComponent.CurrentTarget.CurrentValue;
        if (target == null) return false;
        if (target.Runtime.IsDead.CurrentValue) return false;
        float distance = Vector3.SqrMagnitude(_owner.View.transform.position - target.View.transform.position);
        if (distance > _config.AttackRange * _config.AttackRange) return false;
        if(Time.time < _lastAttackTime + _config.AttackCooldown) return false;
        return true;
    }

    public void Attack() {
        CharacterBrain target = _targetComponent.CurrentTarget.CurrentValue;
        if (target == null) return;
        HealthComponent healthComponent = target.HealthComponent;
        healthComponent.ApplyDamage(_config.Damage);

        Debug.Log($"{_owner.View.name} HP: {_owner.HealthComponent.CurrentHP}");

        _lastAttackTime = Time.time;
    }
}
