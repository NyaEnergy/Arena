using System.Collections.Generic;
using UnityEngine;

public class MedicAllyQueryService {
    private const float HEALTH_EPSILON = 0.0001f;

    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly MedicHealthService _healthService;

    public MedicAllyQueryService(BattlefieldRegistry battlefieldRegistry,
                                 MedicHealthService healthService) {
        _battlefieldRegistry = battlefieldRegistry;
        _healthService = healthService;
    }

    public CharacterBrain FindMostWounded(CharacterBrain medic,
                                          float maximumHealthPercent) {
        IReadOnlyList<CharacterBrain> allies =
            _battlefieldRegistry.GetAllies(
                medic.Runtime.TeamType);

        CharacterBrain result = null;

        float lowestHealthPercent = float.MaxValue;
        float closestSqrDistance = float.MaxValue;

        Vector3 medicPosition = medic.View.transform.position;

        for(int i = 0; i < allies.Count; ++i) {
            CharacterBrain candidate = allies[i];

            if (!IsHealingTarget(medic, candidate)) continue;

            float healthPercent = _healthService.GetHealthPercent(candidate);

            if (healthPercent > maximumHealthPercent) continue;

            float sqrDistance =
                Vector3.SqrMagnitude(medicPosition -
                                     candidate.View.transform.position);

            bool hasLessHealth = healthPercent <
                                 lowestHealthPercent - HEALTH_EPSILON;

            bool hasSameHealthAndIsCloser =
                Mathf.Abs(healthPercent - lowestHealthPercent) <= HEALTH_EPSILON &&
                sqrDistance < closestSqrDistance;

            if (!hasLessHealth &&
               !hasSameHealthAndIsCloser) continue;

            result = candidate;

            lowestHealthPercent = healthPercent;
            closestSqrDistance = sqrDistance;
        }
        return result;
    }

    public CharacterBrain FindClosestLiving(CharacterBrain medic) {
        IReadOnlyList<CharacterBrain> allies =
            _battlefieldRegistry.GetAllies(
                medic.Runtime.TeamType);

        CharacterBrain result = null;
        float closestSqrDistance = float.MaxValue;
        Vector3 medicPosition = medic.View.transform.position;
        for(int i = 0; i < allies.Count; ++i) {
            CharacterBrain candidate = allies[i];
            if (!IsAvailableAlly(medic, candidate)) continue;
            
            float sqrDistance = Vector3.SqrMagnitude(medicPosition - candidate.View.transform.position);

            if (sqrDistance >= closestSqrDistance) continue;

            result = candidate;
            closestSqrDistance = sqrDistance;
        }
        return result;
    }

    public bool IsHealingTarget(CharacterBrain medic,
                                CharacterBrain candidate) {
        return IsAvailableAlly(medic, candidate) &&
            _healthService.IsWounded(candidate);
    }

    private bool IsAvailableAlly(CharacterBrain medic,
                                 CharacterBrain candidate) {
        return medic != null &&
               candidate != null &&
               candidate != medic &&
               !candidate.Runtime.IsDead.CurrentValue &&
               candidate.Runtime.TeamType == medic.Runtime.TeamType;
    }
}
