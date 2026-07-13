using UnityEngine;

public class HunterPositioningService {
    private const float MINIMUM_SQR_DIRECTION = 0.001f;

    private readonly HunterConfig _config;
    private readonly CharacterLineOfSightService _lineOfSightService;

    public HunterPositioningService(HunterConfig config,
                                    CharacterLineOfSightService lineOfSightService) {
        _config = config;
        _lineOfSightService = lineOfSightService;
    }

    public Vector3 GetPositionBehindVanguard(CharacterBrain hunter,
                                             CharacterBrain vanguard,
                                             CharacterBrain threat) {

        Vector3 vanguardPosition = vanguard.View.transform.position;
        Vector3 backDirection = GetDirectionAwayFromThreat(hunter, vanguard, threat);
        Vector3 sideDirection = Vector3.Cross(Vector3.up, backDirection);

        sideDirection.Normalize();

        Vector3 basePosition = vanguardPosition +
                               backDirection *
                               _config.VanguardFollowDistance;

        Vector3 leftPosition = basePosition +
                               sideDirection *
                               _config.VanguardSideOffset;

        Vector3 rightPosition = basePosition -
                                sideDirection *
                                _config.VanguardSideOffset;

        bool hasLeftShot =
            _lineOfSightService.HasClearShotFromPosition(
                    leftPosition, hunter, threat,
                    _config.LineOfSightBlockingLayers,
                    _config.LineOfSightTriggerInteraction);

        bool hasRightShot =
            _lineOfSightService.HasClearShotFromPosition(
                    rightPosition, hunter, threat,
                    _config.LineOfSightBlockingLayers,
                    _config.LineOfSightTriggerInteraction);

        if (hasLeftShot && !hasRightShot)
            return leftPosition;

        if (hasRightShot && !hasLeftShot)
            return rightPosition;

        return GetClosestPosition(
            hunter, leftPosition, rightPosition);
    }

    public Vector3 GetKitePosition(CharacterBrain hunter,
                                   CharacterBrain threat) {

        Vector3 threatPosition = threat.View.transform.position;
        Vector3 direction = hunter.View.transform.position -
                            threatPosition;

        direction.y = 0f;

        if (direction.sqrMagnitude < MINIMUM_SQR_DIRECTION) {
            direction = -threat.View.transform.forward;
            direction.y = 0f;
        }

        direction.Normalize();

        return threatPosition + direction *
               _config.RangedAttackDistanceRange.Max;
    }

    private Vector3 GetDirectionAwayFromThreat(CharacterBrain hunter,
                                               CharacterBrain vanguard,
                                               CharacterBrain threat) {
        Vector3 direction =
            vanguard.View.transform.position -
            threat.View.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude >= MINIMUM_SQR_DIRECTION) {
            return direction.normalized;
        }

        direction =
            hunter.View.transform.position -
            threat.View.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude >= MINIMUM_SQR_DIRECTION) {
            return direction.normalized;
        }

        direction = -threat.View.transform.forward;
        direction.y = 0f;

        return direction.normalized;
    }

    private Vector3 GetClosestPosition(CharacterBrain hunter,
                                       Vector3 firstPosition,
                                       Vector3 secondPosition) {

        Vector3 hunterPosition = hunter.View.transform.position;

        float firstSqrDistance = Vector3.SqrMagnitude(
                hunterPosition - firstPosition);

        float secondSqrDistance = Vector3.SqrMagnitude(
                hunterPosition - secondPosition);

        return firstSqrDistance <= secondSqrDistance ?
            firstPosition : secondPosition;
    }
}