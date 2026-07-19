using UnityEngine;
using Zenject;

public class DemoBattleCameraFollowService : IInitializable,
                                             ILateTickable {
    private const float DEFAULT_FOCUS_DISTANCE = 14f;
    private const float MIN_FOCUS_DISTANCE = 1f;

    private readonly Camera _camera;
    private readonly DemoBattleCombatCenterService _combatCenterService;
    private readonly DemoBattleCameraFollowRuntime _runtime;
    private readonly DemoBattleCameraSmoothingService _smoothingService;
    private readonly DemoBattleCameraMovementService _movementService;
    private readonly DemoBattleCameraZoomService _zoomService;

    public DemoBattleCameraFollowService(Camera camera,
                DemoBattleCombatCenterService combatCenterService,
                DemoBattleCameraFollowRuntime runtime,
                DemoBattleCameraSmoothingService smoothingService,
                DemoBattleCameraMovementService movementService,
                DemoBattleCameraZoomService zoomService) {
        _camera = camera;
        _combatCenterService = combatCenterService;
        _runtime = runtime;
        _smoothingService = smoothingService;
        _movementService = movementService;
        _zoomService = zoomService;
    }

    public void Initialize() {
        if (!TryGetCombatData(out Vector3 center,
                              out float radius)) {
            return;
        }

        InitializeOffset(center);

        _smoothingService.Initialize(
            _runtime, center, radius);
    }

    public void LateTick() {
        if (!TryGetCombatData(out Vector3 center,
                              out float radius)) {
            return;
        }

        if (!_runtime.HasOffset) {
            InitializeOffset(center);
        }

        _smoothingService.Update(_runtime, center, radius);

        Vector3 targetPosition = _runtime.SmoothedCenter +
                                 _runtime.CameraOffset;

        _movementService.Move(_camera, targetPosition, _runtime);
        _zoomService.UpdateZoom(_camera, _runtime.SmoothedRadius, _runtime);
    }

    private bool TryGetCombatData(out Vector3 center,
                                  out float radius) {
        if (_camera == null) {
            center = Vector3.zero;
            radius = 0f;
            return false;
        }

        return _combatCenterService
            .TryGetCenterAndRadius(out center, out radius);
    }

    private void InitializeOffset(Vector3 center) {
        Vector3 toCenter = center -
                           _camera.transform.position;

        float distance = Vector3.Dot(
                toCenter, _camera.transform.forward);

        if (distance < MIN_FOCUS_DISTANCE) {
            distance = toCenter.magnitude;
        }

        if (distance < MIN_FOCUS_DISTANCE) {
            distance = DEFAULT_FOCUS_DISTANCE;
        }

        _runtime.CameraOffset =
            -_camera.transform.forward.normalized *
             distance;

        _runtime.HasOffset = true;
    }
}