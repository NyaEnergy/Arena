using UnityEngine;

public class ControllerPositioningService {
    private const float DISTANCE_BUFFER = 0.2f;
    private const float MINIMUM_DIRECTION = 0.001f;

    private readonly ControllerConfig _config;

    public ControllerPositioningService( ControllerConfig config) {
        _config = config;
    }

    public bool Tick(CharacterBrain controller,
                     CharacterBrain target) {

        if (controller?.View == null ||
            target?.View == null) {
                return false;
        }

        Vector3 controllerPosition =
            controller.View.transform.position;

        Vector3 targetPosition =
            target.View.transform.position;

        Vector3 difference = controllerPosition -
                             targetPosition;

        difference.y = 0f;

        float minimum =
            Mathf.Max(0f, _config.ControlDistanceRange.Min);

        float maximum =
            Mathf.Max(minimum, _config.ControlDistanceRange.Max);

        float sqrDistance =
            difference.sqrMagnitude;

        if (sqrDistance > maximum * maximum) {

            controller.MovementComponent
                .MoveToDistance(targetPosition,
                                Mathf.Max(0f, maximum - DISTANCE_BUFFER),
                                1f);

            return false;
        }

        if (sqrDistance < minimum * minimum) {

            MoveAway(controller, target, difference);

            return false;
        }

        controller.MovementComponent.Stop();
        return true;
    }

    private void MoveAway(CharacterBrain controller,
                          CharacterBrain target,
                          Vector3 direction) {

        if (direction.sqrMagnitude < MINIMUM_DIRECTION) {

            direction = -target.View.transform.forward;
            direction.y = 0f;
        }

        if (direction.sqrMagnitude < MINIMUM_DIRECTION) {

            direction = Vector3.back;
        }

        direction.Normalize();

        Vector3 position =
            controller.View
                      .transform
                      .position + direction * Mathf.Max(0f, _config.RetreatStepDistance);

        controller.MovementComponent
                  .MoveToPosition(position);
    }
}