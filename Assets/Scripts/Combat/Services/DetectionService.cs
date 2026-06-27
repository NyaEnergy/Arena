using System.Collections.Generic;
using UnityEngine;

public class DetectionService {
    private readonly BattlefieldRegistry _battlefieldRegistry;
    
    public DetectionService(BattlefieldRegistry battlefieldRegistry) {
        _battlefieldRegistry = battlefieldRegistry;
    }

    public CharacterBrain FindClosestTarget(CharacterBrain owner) {
        IReadOnlyList<CharacterBrain> targets = _battlefieldRegistry.GetEnemies(owner.Runtime.TeamType);
        CharacterBrain closestTarget = null;
        float closestDistance = float.MaxValue;
        Vector3 ownerPosition = owner.View.transform.position;

        for (int i = 0; i < targets.Count; ++i) {
            CharacterBrain target = targets[i];
            if (target == null) continue;

            if (target.Runtime.IsDead.CurrentValue) continue;

            float sqrDistance = Vector3.SqrMagnitude(ownerPosition - target.View.transform.position);
            if (sqrDistance >= closestDistance) continue;

            closestDistance = sqrDistance;
            closestTarget = target;
        }

        return closestTarget;
    }
}
