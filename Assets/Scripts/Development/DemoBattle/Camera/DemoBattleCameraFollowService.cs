using UnityEngine;
using Zenject;

public class DemoBattleCameraFollowService : IInitializable,
                                             ILateTickable {
    private const float DEFAULT_OFFSET_Y = 12f;
    private const float DEFAULT_OFFSET_Z = -10f;
    private const float FOCUS_HEIGHT = 1.2f;
    private const float MIN_OFFSET_SQR_MAGNITUDE = 0.01f;

    private readonly Camera _camera;
    private readonly DemoBattleCombatCenterService _combatCenterService;
    private readonly DemoBattleCameraFollowRuntime _runtime;
    private readonly DemoBattleCameraSmoothingService _smoothingService;
    private readonly DemoBattleCameraMovementService _movementService;
    private readonly DemoBattleCameraRotationService _rotationService;
    private readonly DemoBattleCameraZoomService _zoomService;

    public DemoBattleCameraFollowService(Camera camera,
                                         DemoBattleCombatCenterService combatCenterService,
                                         DemoBattleCameraFollowRuntime runtime,
                                         DemoBattleCameraSmoothingService smoothingService,
                                         DemoBattleCameraMovementService movementService,
                                         DemoBattleCameraRotationService rotationService,
                                         DemoBattleCameraZoomService zoomService) {
        _camera = camera;
        _combatCenterService = combatCenterService;
        _runtime = runtime;
        _smoothingService = smoothingService;
        _movementService = movementService;
        _rotationService = rotationService;
        _zoomService = zoomService;
    }

    public void Initialize() {
        if (!TryGetCombatData(out Vector3 center,
                              out float radius)) {
            return;
        }

        InitializeOffset(center);

        _smoothingService.Initialize(_runtime,
                                     center,
                                     radius);
    }

    public void LateTick() {
        if (!TryGetCombatData(out Vector3 center,
                              out float radius)) {
            return;
        }

        if (!_runtime.HasOffset) {
            InitializeOffset(center);
        }

        _smoothingService.Update(_runtime,
                                 center,
                                 radius);

        Vector3 focusPoint = _runtime.SmoothedCenter +
                             Vector3.up * FOCUS_HEIGHT;

        Vector3 targetPosition = _runtime.SmoothedCenter +
                                 _runtime.CameraOffset;

        _movementService.Move(_camera,
                               targetPosition,
                              _runtime);

        _rotationService.Rotate(_camera,
                                 focusPoint);

        _zoomService.UpdateZoom(_camera,
                                _runtime.SmoothedRadius,
                                _runtime);
    }

    private bool TryGetCombatData(out Vector3 center,
                                  out float radius) {
        if (_camera == null) {
            center = Vector3.zero;
            radius = 0f;
            return false;
        }

        return _combatCenterService.TryGetCenterAndRadius(
                out center, out radius);
    }

    private void InitializeOffset(Vector3 center) {
        Vector3 offset = _camera.transform.position - center;

        if (offset.sqrMagnitude < MIN_OFFSET_SQR_MAGNITUDE) {
            offset = new Vector3(0f, DEFAULT_OFFSET_Y,
                                     DEFAULT_OFFSET_Z);
        }

        _runtime.CameraOffset = offset;
        _runtime.HasOffset = true;
    }
}