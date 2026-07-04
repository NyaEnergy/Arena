using UnityEngine;

public class MedicHealingService {
    private readonly MedicConfig _config;

    public MedicHealingService(MedicConfig config) {
        _config = config;
    }

    public bool TryHeal(CharacterBrain medic,
                        CharacterBrain target) {
        if(medic == null || target == null ||
           target.Runtime.IsDead.CurrentValue) return false;

        float currentHP = target.Runtime
                                .CurrentHP
                                .CurrentValue;

        if (currentHP >= target.Config.MaxHP) return false;

        float sqrDistance = Vector3.SqrMagnitude(medic.View.transform.position -
                                                 target.View.transform.position);

        float sqrHealingDistance = _config.HealingDistance *
                                   _config.HealingDistance;

        if (sqrDistance > sqrHealingDistance) return false;

        float healing = _config.HealingPerSecond * Time.deltaTime;
        target.HealthComponent.ApplyHealing(healing);
        return true;
    }
}
