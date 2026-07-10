using UnityEngine;
using Zenject;

public class DemoBattleCameraFollowService : IInitializable,
                                             ILateTickable {
    private const float DEFAULT_OFFSET_X = 0f;
    private const float DEFAULT_OFFSET_Y = 12f;
    private const float DEFAULT_OFFSET_Z = -10f;

    private const float FOCUS_HEIGHT = 1.2f;

    private const float POSITION_STOP_DISTANCE = 0.03f;
    private const float POSITION_STOP_SPEED = 0.03f;

    private const float DISTANCE_FOR_MAX_SPEED = 8f;
    private const float MAX_MOVE_SPEED = 10f;

    private const float ACCELERATION_PER_SECOND = 18f;
    private const float DECELERATION_PER_SECOND = 24f;

    private const float CENTER_SMOOTH_TIME = 0.35f;
    private const float MAX_CENTER_FOLLOW_SPEED = 8f;

    private const float RADIUS_SMOOTH_TIME = 0.35f;
    private const float MAX_RADIUS_CHANGE_SPEED = 8f;

    private const float MAX_ROTATION_SPEED = 120f;

    private const float MIN_RADIUS_FOR_ZOOM = 4f;
    private const float MAX_RADIUS_FOR_ZOOM = 13f;

    private const float MIN_ORTHOGRAPHIC_SIZE = 6f;
    private const float MAX_ORTHOGRAPHIC_SIZE = 13f;

    private const float MIN_FIELD_OF_VIEW = 45f;
    private const float MAX_FIELD_OF_VIEW = 68f;

    private const float ZOOM_SMOOTH_TIME = 0.3f;
    private const float MAX_ZOOM_SPEED = 8f;

    private const float MIN_OFFSET_SQR_MAGNITUDE = 0.01f;

    private readonly Camera _camera;
    private readonly DemoBattleCombatCenterService _combatCenterService;

    private Vector3 _cameraOffset;
    private Vector3 _moveVelocity;
    private Vector3 _centerVelocity;

    private float _smoothedRadius;
    private float _radiusVelocity;
    private float _zoomVelocity;

    private bool _hasOffset;
    private bool _hasSmoothedCenter;

    private Vector3 _smoothedCenter;

    public DemoBattleCameraFollowService(Camera camera,
                                         DemoBattleCombatCenterService combatCenterService) {
        _camera = camera;
        _combatCenterService = combatCenterService;
    }

    public void Initialize() {
        TryInitializeOffset();
    }

    public void LateTick() {
        if (_camera == null) return;

        if (!_combatCenterService.TryGetCenterAndRadius(
                out Vector3 rawCenter,
                out float rawRadius)) {

            return;
        }

        if (!_hasOffset) {
            InitializeOffset(rawCenter);
        }

        UpdateSmoothedCombatData(rawCenter, rawRadius);

        Vector3 focusPoint =
            _smoothedCenter + Vector3.up * FOCUS_HEIGHT;

        Vector3 targetPosition =
            _smoothedCenter + _cameraOffset;

        MoveToTarget(targetPosition);
        RotateToFocus(focusPoint);
        UpdateZoom(_smoothedRadius);
    }

    private void TryInitializeOffset() {
        if (_camera == null) return;

        if (!_combatCenterService.TryGetCenterAndRadius(
                out Vector3 center,
                out float radius)) {

            return;
        }

        InitializeOffset(center);
        InitializeSmoothedCombatData(center, radius);
    }

    private void InitializeOffset(Vector3 center) {
        Vector3 offset =
            _camera.transform.position - center;

        if (offset.sqrMagnitude < MIN_OFFSET_SQR_MAGNITUDE) {
            offset =
                new Vector3(
                    DEFAULT_OFFSET_X,
                    DEFAULT_OFFSET_Y,
                    DEFAULT_OFFSET_Z);
        }

        _cameraOffset = offset;
        _hasOffset = true;
    }

    private void InitializeSmoothedCombatData(Vector3 center,
                                             float radius) {
        _smoothedCenter = center;
        _smoothedRadius = radius;
        _centerVelocity = Vector3.zero;
        _radiusVelocity = 0f;
        _hasSmoothedCenter = true;
    }

    private void UpdateSmoothedCombatData(Vector3 rawCenter,
                                          float rawRadius) {
        if (!_hasSmoothedCenter) {
            InitializeSmoothedCombatData(rawCenter, rawRadius);
            return;
        }

        _smoothedCenter =
            Vector3.SmoothDamp(
                _smoothedCenter,
                rawCenter,
                ref _centerVelocity,
                CENTER_SMOOTH_TIME,
                MAX_CENTER_FOLLOW_SPEED,
                Time.deltaTime);

        _smoothedRadius =
            Mathf.SmoothDamp(
                _smoothedRadius,
                rawRadius,
                ref _radiusVelocity,
                RADIUS_SMOOTH_TIME,
                MAX_RADIUS_CHANGE_SPEED,
                Time.deltaTime);
    }

    private void MoveToTarget(Vector3 targetPosition) {
        Transform cameraTransform =
            _camera.transform;

        Vector3 currentPosition =
            cameraTransform.position;

        Vector3 toTarget =
            targetPosition - currentPosition;

        float distance =
            toTarget.magnitude;

        float stopSpeedSqr =
            POSITION_STOP_SPEED * POSITION_STOP_SPEED;

        if (distance <= POSITION_STOP_DISTANCE &&
            _moveVelocity.sqrMagnitude <= stopSpeedSqr) {

            _moveVelocity = Vector3.zero;
            return;
        }

        if (distance <= 0f) return;

        Vector3 direction =
            toTarget / distance;

        float distanceFactor =
            Mathf.Clamp01(distance / DISTANCE_FOR_MAX_SPEED);

        float desiredSpeed =
            MAX_MOVE_SPEED * distanceFactor;

        float currentSpeed =
            _moveVelocity.magnitude;

        float brakingDistance =
            GetBrakingDistance(currentSpeed);

        if (brakingDistance > POSITION_STOP_DISTANCE &&
            distance <= brakingDistance) {

            float brakingFactor =
                Mathf.Clamp01(distance / brakingDistance);

            desiredSpeed *= brakingFactor;
        }

        Vector3 desiredVelocity =
            direction * desiredSpeed;

        float speedChangeRate =
            desiredSpeed > currentSpeed ?
                ACCELERATION_PER_SECOND :
                DECELERATION_PER_SECOND;

        _moveVelocity =
            Vector3.MoveTowards(
                _moveVelocity,
                desiredVelocity,
                speedChangeRate * Time.deltaTime);

        _moveVelocity =
            Vector3.ClampMagnitude(
                _moveVelocity,
                MAX_MOVE_SPEED);

        Vector3 movement =
            _moveVelocity * Time.deltaTime;

        if (movement.sqrMagnitude >= toTarget.sqrMagnitude) {
            cameraTransform.position = targetPosition;
            _moveVelocity = Vector3.zero;
            return;
        }

        cameraTransform.position =
            currentPosition + movement;
    }

    private float GetBrakingDistance(float currentSpeed) {
        if (DECELERATION_PER_SECOND <= 0f) return 0f;

        return currentSpeed *
               currentSpeed /
               (2f * DECELERATION_PER_SECOND);
    }

    private void RotateToFocus(Vector3 focusPoint) {
        Transform cameraTransform =
            _camera.transform;

        Vector3 lookDirection =
            focusPoint - cameraTransform.position;

        if (lookDirection.sqrMagnitude <= 0f) return;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                lookDirection.normalized,
                Vector3.up);

        cameraTransform.rotation =
            Quaternion.RotateTowards(
                cameraTransform.rotation,
                targetRotation,
                MAX_ROTATION_SPEED * Time.deltaTime);
    }

    private void UpdateZoom(float radius) {
        float zoomFactor =
            Mathf.InverseLerp(
                MIN_RADIUS_FOR_ZOOM,
                MAX_RADIUS_FOR_ZOOM,
                radius);

        if (_camera.orthographic) {
            float targetSize =
                Mathf.Lerp(
                    MIN_ORTHOGRAPHIC_SIZE,
                    MAX_ORTHOGRAPHIC_SIZE,
                    zoomFactor);

            _camera.orthographicSize =
                Mathf.SmoothDamp(
                    _camera.orthographicSize,
                    targetSize,
                    ref _zoomVelocity,
                    ZOOM_SMOOTH_TIME,
                    MAX_ZOOM_SPEED,
                    Time.deltaTime);

            return;
        }

        float targetFieldOfView =
            Mathf.Lerp(
                MIN_FIELD_OF_VIEW,
                MAX_FIELD_OF_VIEW,
                zoomFactor);

        _camera.fieldOfView =
            Mathf.SmoothDamp(
                _camera.fieldOfView,
                targetFieldOfView,
                ref _zoomVelocity,
                ZOOM_SMOOTH_TIME,
                MAX_ZOOM_SPEED,
                Time.deltaTime);
    }
}