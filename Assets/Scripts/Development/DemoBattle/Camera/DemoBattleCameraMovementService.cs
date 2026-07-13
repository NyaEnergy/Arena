using UnityEngine;

public class DemoBattleCameraMovementService {
    private const float POSITION_STOP_DISTANCE = 0.03f;
    private const float POSITION_STOP_SPEED = 0.03f;
    private const float DISTANCE_FOR_MAX_SPEED = 8f;
    private const float MAX_MOVE_SPEED = 10f;
    private const float ACCELERATION_PER_SECOND = 18f;
    private const float DECELERATION_PER_SECOND = 24f;

    public void Move(Camera camera,
                     Vector3 targetPosition,
                     DemoBattleCameraFollowRuntime runtime) {
        Transform cameraTransform = camera.transform;
        Vector3 currentPosition = cameraTransform.position;
        Vector3 toTarget = targetPosition - currentPosition;
        float distance = toTarget.magnitude;

        float stopSpeedSqr =
            POSITION_STOP_SPEED * POSITION_STOP_SPEED;

        if (distance <= POSITION_STOP_DISTANCE &&
            runtime.MoveVelocity.sqrMagnitude <= stopSpeedSqr) {
            runtime.MoveVelocity = Vector3.zero;
            return;
        }

        if (distance <= 0f) return;

        Vector3 direction = toTarget / distance;

        float distanceFactor =
            Mathf.Clamp01(distance / DISTANCE_FOR_MAX_SPEED);

        float desiredSpeed =
            MAX_MOVE_SPEED * distanceFactor;

        float currentSpeed =
            runtime.MoveVelocity.magnitude;

        float brakingDistance =
            GetBrakingDistance(currentSpeed);

        if (brakingDistance > POSITION_STOP_DISTANCE &&
            distance <= brakingDistance) {
            desiredSpeed *= Mathf.Clamp01(distance / brakingDistance);
        }

        Vector3 desiredVelocity = direction * desiredSpeed;

        float speedChangeRate = desiredSpeed > currentSpeed ?
            ACCELERATION_PER_SECOND : DECELERATION_PER_SECOND;

        runtime.MoveVelocity = Vector3.MoveTowards(
                runtime.MoveVelocity,
                desiredVelocity,
                speedChangeRate * Time.deltaTime);

        runtime.MoveVelocity = Vector3.ClampMagnitude(
                runtime.MoveVelocity,
                MAX_MOVE_SPEED);

        Vector3 movement = runtime.MoveVelocity * Time.deltaTime;

        if (movement.sqrMagnitude >= toTarget.sqrMagnitude) {
            cameraTransform.position = targetPosition;
            runtime.MoveVelocity = Vector3.zero;
            return;
        }

        cameraTransform.position = currentPosition + movement;
    }

    private float GetBrakingDistance(float currentSpeed) {
        return DECELERATION_PER_SECOND > 0f ?
            currentSpeed * currentSpeed /
            (2f * DECELERATION_PER_SECOND) : 0f;
    }
}