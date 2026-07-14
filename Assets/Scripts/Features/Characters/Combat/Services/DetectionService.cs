using System.Collections.Generic;
using UnityEngine;

public class DetectionService {
    private readonly BattlefieldRegistry _registry;

    public DetectionService( BattlefieldRegistry registry) {
        _registry = registry;
    }

    public CharacterBrain FindClosestTarget( CharacterBrain owner) {
        if (owner == null ||
            owner.View == null ||
            owner.Runtime == null) {
            return null;
        }

        IReadOnlyList<CharacterBrain> targets =
            _registry.GetEnemies(owner.Runtime.TeamType);

        CharacterBrain closest = null;

        float closestSqrDistance = float.MaxValue;

        Vector3 ownerPosition = owner.View.transform.position;

        for (int i = 0; i < targets.Count; i++) {

            CharacterBrain target = targets[i];

            if (!IsAvailable(target))
                continue;

            Vector3 difference = target.View.transform.position -
                                 ownerPosition;

            difference.y = 0f;

            float sqrDistance = difference.sqrMagnitude;

            if (sqrDistance >= closestSqrDistance)
                continue;

            closestSqrDistance = sqrDistance;

            closest = target;
        }

        return closest;
    }

    private bool IsAvailable( CharacterBrain target) {
        return target != null &&
               target.View != null &&
               target.Runtime != null &&
               !target.Runtime
                      .IsDead
                      .CurrentValue;
    }
}