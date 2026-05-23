using System.Collections.Generic;
using UnityEngine;

public class DetectionService {
    private readonly BattlefieldRegistry _battlefieldRegistry;
    
    private readonly float _detectionRadius = 12f;

    public DetectionService(BattlefieldRegistry battlefieldRegistry) {
        _battlefieldRegistry = battlefieldRegistry;
    }

    public CharacterBrain FindClosestTarget(CharacterBrain owner) {
        IReadOnlyList<CharacterBrain> targets = _battlefieldRegistry.GetEnemies(owner.Runtime.TeamType);
        CharacterBrain closestTarget = null;
        float closestDistance = float.MaxValue;
        Vector3 ownerPosition = owner.View.transform.position;

        for (int i = 0; i < targets.Count; ++i) {
            if (targets[i].Runtime.IsDead.CurrentValue) continue;
            float distance = Vector3.SqrMagnitude(ownerPosition - targets[i].View.transform.position);
            if (distance > _detectionRadius * _detectionRadius) continue;

            if(distance < closestDistance) {
                closestDistance = distance;
                closestTarget = targets[i];
            }
        }

        return closestTarget;
    }
}
