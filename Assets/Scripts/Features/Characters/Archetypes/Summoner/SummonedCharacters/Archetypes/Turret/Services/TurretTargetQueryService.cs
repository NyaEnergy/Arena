using System.Collections.Generic;
using UnityEngine;

public class TurretTargetQueryService {
    private readonly BattlefieldRegistry _registry;
    private readonly CharacterLineOfSightService _lineOfSightService;

    public TurretTargetQueryService(BattlefieldRegistry registry,
                                    CharacterLineOfSightService lineOfSightService) {
        _registry = registry;
        _lineOfSightService = lineOfSightService;
    }

    public CharacterBrain FindClosest(CharacterBrain turret,
                                      TurretConfig config) {

        if (turret == null || config == null) return null;

        IReadOnlyList<CharacterBrain> targets =
            _registry.GetEnemies(
                turret.Runtime.TeamType);

        CharacterBrain closest = null;
        float closestDistance = float.MaxValue;
        float minimum = config.AttackDistanceRange.Min;
        float maximum = config.AttackDistanceRange.Max;

        Vector3 position = turret.View.transform.position;

        for (int i = 0; i < targets.Count; i++) {
            CharacterBrain target = targets[i];

            if (target == null ||
                target.Runtime.IsDead.CurrentValue) continue;

            float sqrDistance = Vector3.SqrMagnitude(
                    position - target.View.transform.position);

            if (sqrDistance < minimum * minimum ||
                sqrDistance > maximum * maximum ||
                sqrDistance >= closestDistance) continue;

            if (!_lineOfSightService.HasClearShot(
                    turret, target,
                    config.LineOfSightBlockingLayers,
                    config.LineOfSightTriggerInteraction)) continue;

            closest = target;
            closestDistance = sqrDistance;
        }

        return closest;
    }
}