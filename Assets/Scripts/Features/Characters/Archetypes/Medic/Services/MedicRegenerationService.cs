using UnityEngine;

public sealed class MedicRegenerationService {
    private readonly MedicConfig _config;

    public MedicRegenerationService(MedicConfig config) {
        _config = config;
    }

    public bool Tick(CharacterBrain medic) {
        if (medic?.Runtime == null ||
            medic.Config == null ||
            medic.Runtime.TeamType != TeamType.Ally ||
            medic.Runtime.IsDead.CurrentValue) {
            return false;
        }

        float currentHP = medic.Runtime.CurrentHP.CurrentValue;

        if (currentHP >= medic.Config.MaxHP ||
            Time.time < medic.HealthComponent.LastDamageTime +
                        _config.RegenerationDelay) {
            return false;
        }

        medic.HealthComponent.ApplyHealing(
            _config.RegenerationPerSecond * Time.deltaTime);

        return true;
    }
}
