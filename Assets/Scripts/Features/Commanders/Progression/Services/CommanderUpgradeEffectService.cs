using System;
using UnityEngine;

public sealed class CommanderUpgradeEffectService {
    private const float MINIMUM_REFILL_INTERVAL = 0.1f;

    private readonly CommanderProgressionService _progressionService;

    public CommanderUpgradeEffectService(
        CommanderProgressionService progressionService) {
        _progressionService = progressionService ??
            throw new ArgumentNullException(nameof(progressionService));
    }

    public float GetMedicHealingPerSecond(TeamType teamType,
                                          float baseHealingPerSecond) {
        return Mathf.Max(0f, baseHealingPerSecond) *
               GetMultiplier(teamType,
                             CommanderUpgradeEffectType.MedicHealing);
    }

    public float GetControllerSlowMultiplier(TeamType teamType,
                                             float baseSlowMultiplier) {
        float slowMultiplier = Mathf.Clamp01(baseSlowMultiplier);
        float slowStrength = 1f - slowMultiplier;

        float upgradedStrength = Mathf.Clamp01(
            slowStrength *
            GetMultiplier(
                teamType,
                CommanderUpgradeEffectType.ControllerSlowStrength));

        return 1f - upgradedStrength;
    }

    public float GetEnemyConveyorRefillInterval(float baseInterval) {
        float multiplier = GetMultiplier(
            TeamType.Enemy,
            CommanderUpgradeEffectType.EnemyConveyorRefillInterval);

        return Mathf.Max(MINIMUM_REFILL_INTERVAL,
                         Mathf.Max(0f, baseInterval) * multiplier);
    }

    private float GetMultiplier(TeamType teamType,
                                CommanderUpgradeEffectType effectType) {
        if (effectType == CommanderUpgradeEffectType.None) return 1f;

        float multiplier = 1f;

        for (int i = 0; i < _progressionService.Nodes.Count; ++i) {
            CommanderProgressionRuntime runtime =
                _progressionService.Nodes[i];

            if (!runtime.IsUnlocked ||
                runtime.Commander.TeamType != teamType ||
                runtime.Node.NodeType != CommanderProgressionNodeType.Upgrade ||
                runtime.Node.UpgradeEffectType != effectType) {
                continue;
            }

            multiplier *= runtime.Node.UpgradeEffectMultiplier;
        }

        return multiplier;
    }
}
