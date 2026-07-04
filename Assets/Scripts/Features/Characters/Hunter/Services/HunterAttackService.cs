using UnityEngine;

public class HunterAttackService {
    private readonly HunterConfig _config;
    private readonly CharacterLineOfSightService _lineOfSightService;

    public HunterAttackService(HunterConfig config,
                               CharacterLineOfSightService lineOfSightService) {
        _config = config;
        _lineOfSightService = lineOfSightService;
    }

    public bool TryRangedAttack(CharacterBrain hunter,
                                CharacterBrain target,
                                HunterRuntime runtime) {
        if (!CanAttack(hunter, target, runtime,
                _config.RangedAttackDistanceRange)) {
            return false;
        }

        if (!_lineOfSightService.HasClearShot(hunter, target,
                                              _config.LineOfSightBlockingLayers,
                                              _config.LineOfSightTriggerInteraction)) {
            return false;
        }

        ApplyAttack(target, runtime,
                    _config.RangedDamage,
                    _config.RangedAttackCooldown);

        PlayRangedAttack(hunter, target);

        return true;
    }

    public bool TryMeleeAttack(CharacterBrain hunter,
                               CharacterBrain target,
                               HunterRuntime runtime) {

        if (!CanAttack(hunter, target, runtime,
                _config.MeleeAttackDistanceRange)) {
            return false;
        }

        ApplyAttack(target, runtime,
            _config.MeleeDamage,
            _config.MeleeAttackCooldown);

        PlayMeleeAttack(hunter, target);

        return true;
    }

    private bool CanAttack(CharacterBrain hunter,
                           CharacterBrain target,
                           HunterRuntime runtime,
                           Range attackDistanceRange) {

        if (hunter == null ||
            target == null ||
            target.Runtime.IsDead.CurrentValue ||
            Time.time < runtime.NextAttackTime) {

            return false;
        }

        float sqrDistance = Vector3.SqrMagnitude(
                hunter.View.transform.position -
                target.View.transform.position);

        Range sqrDistanceRange = new(attackDistanceRange.Min * attackDistanceRange.Min,
                                     attackDistanceRange.Max * attackDistanceRange.Max);


        return sqrDistance >= sqrDistanceRange.Min &&
               sqrDistance <= sqrDistanceRange.Max;
    }

    private void ApplyAttack(CharacterBrain target,
                             HunterRuntime runtime,
                             float damage,
                             float cooldown) {

        target.HealthComponent.ApplyDamage(damage);
        runtime.NextAttackTime = Time.time + cooldown;
    }

    private void PlayRangedAttack(CharacterBrain hunter,
                                  CharacterBrain target) {
        HunterView hunterView = hunter.View as HunterView;
        hunterView?.PlayRangedAttack(target.View);
        target.View.PlayHit();
    }

    private void PlayMeleeAttack(CharacterBrain hunter,
                                 CharacterBrain target) {
        HunterView hunterView = hunter.View as HunterView;
        hunterView?.PlayMeleeAttack(target.View);
        target.View.PlayHit();
    }
}