using UnityEngine;

public class CharacterLineOfSightService {
    private const int HIT_BUFFER_SIZE = 32;
    private const float MINIMUM_SQR_DISTANCE = 0.0001f;
    private const float DEFAULT_AIM_HEIGHT = 1f;

    private readonly RaycastHit[] _hitBuffer = new RaycastHit[HIT_BUFFER_SIZE];

    public bool HasClearShot(CharacterBrain shooter,
                             CharacterBrain target,
                             LayerMask blockingLayers,
                             QueryTriggerInteraction triggerInteraction) {
        if (shooter == null || target == null) return false;
        return HasClearShotFromPosition(
            shooter.View.transform.position,
            shooter,
            target,
            blockingLayers,
            triggerInteraction);
    }

    public bool HasClearShotFromPosition(Vector3 shooterPosition,
                                          CharacterBrain shooter,
                                          CharacterBrain target,
                                          LayerMask blockingLayers,
                                          QueryTriggerInteraction triggerInteraction) {
        if (shooter == null ||
            target == null ||
            shooter.View == null ||
            target.View == null ||
            target.Runtime.IsDead.CurrentValue) return false;

        Vector3 origin = GetAimPoint(shooter, shooterPosition);
        Vector3 destination = GetAimPoint(target, target.View.transform.position);
        Vector3 direction = destination - origin;
        
        float sqrDistance = direction.sqrMagnitude;

        if (sqrDistance <= MINIMUM_SQR_DISTANCE) return true;

        float distance = Mathf.Sqrt(sqrDistance);
        direction /= distance;

        int hitCount = Physics.RaycastNonAlloc(origin,
                                               direction,
                                               _hitBuffer,
                                               distance,
                                               blockingLayers,
                                               triggerInteraction);

        for (int i = 0; i < hitCount; ++i) {
            Collider hitCollider = _hitBuffer[i].collider;

            if (hitCollider == null) continue;

            if (BelongsToCharacter(hitCollider, shooter)) continue;
            if (BelongsToCharacter(hitCollider, target)) continue;

            return false;
        }

        return hitCount < _hitBuffer.Length;
    }

    private Vector3 GetAimPoint(CharacterBrain character,
                                Vector3 rootPosition) {

        float aimHeight = DEFAULT_AIM_HEIGHT;

        if (character.View.Agent != null) {
            aimHeight = character.View.Agent.baseOffset +
                        character.View.Agent.height * 0.5f;
        }

        return rootPosition + Vector3.up * aimHeight;
    }

    private bool BelongsToCharacter(Collider collider,
                                    CharacterBrain character) {

        Transform characterTransform = character.View.transform;
        Transform colliderTransform = collider.transform;

        return colliderTransform == characterTransform ||
               colliderTransform.IsChildOf(characterTransform);
    }

}
