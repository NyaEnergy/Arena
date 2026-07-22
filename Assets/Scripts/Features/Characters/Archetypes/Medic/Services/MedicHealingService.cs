using UnityEngine;

public class MedicHealingService {
    private readonly MedicConfig _config;
    private readonly CommanderQuestService _questService;
    private readonly CommanderUpgradeEffectService _upgradeEffectService;

    public MedicHealingService(MedicConfig config,
                               CommanderQuestService questService,
                               CommanderUpgradeEffectService upgradeEffectService) {
        _config = config;
        _questService = questService;
        _upgradeEffectService = upgradeEffectService;
    }

    public bool TryHeal(CharacterBrain medic,
                        CharacterBrain target) {
        if (medic == null || target == null ||
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

        float healingPerSecond =
            _upgradeEffectService.GetMedicHealingPerSecond(
                medic.Runtime.TeamType,
                _config.HealingPerSecond);

        float healing = Mathf.Min(
            healingPerSecond * Time.deltaTime,
            target.Config.MaxHP - currentHP);

        target.HealthComponent.ApplyHealing(healing);

        _questService.Report(new CommanderQuestEvent(
            CommanderQuestEventType.HealingDone,
            medic.Runtime.TeamType,
            CharacterType.Medic,
            healing));

        return true;
    }
}