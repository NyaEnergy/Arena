using ConveyorWars.Presentation.Input;
using Unity.Cinemachine;
using UnityEngine;

namespace ConveyorWars.Presentation.Cameras {
    public sealed class SandboxCameraController {
        private const float RETARGET_COMPLETE_DISTANCE_SQR = 0.0025f;

        private readonly GameplayInputRuntime _inputRuntime;
        private readonly GameplayControlConfig _config;
        private readonly CinemachineView _view;

        private Transform _trackingTarget;
        private Vector3 _trackingVelocity;
        private bool _isRetargeting;

        private float _targetHorizontalAxis;
        private float _horizontalVelocity;

        private float _targetRadialAxis;
        private float _radialVelocity;

        private bool _isInitialized;


        public SandboxCameraController(GameplayInputRuntime inputRuntime,
                                       GameplayControlConfig config,
                                       CinemachineView view) {
            _inputRuntime = inputRuntime;
            _config = config;
            _view = view;
        }

        public bool TryInitialize(Transform trackingTarget) {
            if (!CanUseTrackingTarget(trackingTarget)) return false;

            _trackingTarget = trackingTarget;
            _view.TrackingTarget.position = trackingTarget.position;

            SetCameraTarget(_view.TrackingTarget);

            _targetHorizontalAxis =
                _view.OrbitalFollow.HorizontalAxis.Value;

            _targetRadialAxis =
                _view.OrbitalFollow.RadialAxis.Value;

            _isInitialized = true;
            return true;
        }

        public bool TrySetTrackingTarget(Transform trackingTarget) {
            if (!_isInitialized ||
                !CanUseTrackingTarget(trackingTarget)) {
                return false;
            }

            if (_trackingTarget == trackingTarget) return true;

            _trackingTarget = trackingTarget;
            _trackingVelocity = Vector3.zero;
            _isRetargeting = true;

            return true;
        }

        public void Tick(float deltaTime) {
            if (!_isInitialized || deltaTime <= 0f) return;

            UpdateTrackingTarget(deltaTime);

            ReadRotation();
            ReadZoom();

            UpdateRotation(deltaTime);
            UpdateZoom(deltaTime);
        }
        private void UpdateTrackingTarget(float deltaTime) {
            if (_trackingTarget == null) return;

            if (!_isRetargeting) {
                _view.TrackingTarget.position =
                    _trackingTarget.position;
                return;
            }

            _view.TrackingTarget.position =
                Vector3.SmoothDamp(
                    _view.TrackingTarget.position,
                    _trackingTarget.position,
                    ref _trackingVelocity,
                    _config.CameraRetargetSmoothTime,
                    Mathf.Infinity,
                    deltaTime);

            if ((_view.TrackingTarget.position -
                 _trackingTarget.position).sqrMagnitude >
                RETARGET_COMPLETE_DISTANCE_SQR) {
                return;
            }

            _view.TrackingTarget.position =
                _trackingTarget.position;

            _trackingVelocity = Vector3.zero;
            _isRetargeting = false;
        }

        private void ReadRotation() {
            if (!_inputRuntime.IsCameraRotateHeld()) return;

            Vector2 pointerDelta = _inputRuntime.ReadPointerDelta();

            if (Mathf.Approximately(pointerDelta.x, 0f)) return;

            InputAxis horizontalAxis = _view.OrbitalFollow.HorizontalAxis;

            _targetHorizontalAxis = horizontalAxis.ClampValue(
                                        _targetHorizontalAxis +
                                        pointerDelta.x *
                                        _config.CameraRotateSensitivity);
        }

        private void ReadZoom() {
            float zoomInput = _inputRuntime.ReadCameraZoom();

            if (Mathf.Approximately(zoomInput, 0f)) return;

            InputAxis radialAxis = _view.OrbitalFollow.RadialAxis;

            _targetRadialAxis = radialAxis.ClampValue(
                                    _targetRadialAxis -
                                    zoomInput *
                                    _config.CameraZoomSensitivity);
        }

        private void UpdateRotation(float deltaTime) {
            InputAxis horizontalAxis = _view.OrbitalFollow.HorizontalAxis;

            horizontalAxis.Value = Mathf.SmoothDampAngle(
                                        horizontalAxis.Value,
                                        _targetHorizontalAxis,
                                        ref _horizontalVelocity,
                                        _config.CameraRotateSmoothTime,
                                        Mathf.Infinity,
                                        deltaTime);

            horizontalAxis.Value = horizontalAxis.ClampValue(horizontalAxis.Value);

            _view.OrbitalFollow.HorizontalAxis = horizontalAxis;
        }

        private void UpdateZoom(float deltaTime) {
            InputAxis radialAxis = _view.OrbitalFollow.RadialAxis;

            radialAxis.Value = Mathf.SmoothDamp(
                                    radialAxis.Value,
                                    _targetRadialAxis,
                                    ref _radialVelocity,
                                    _config.CameraZoomSmoothTime,
                                    Mathf.Infinity,
                                    deltaTime);

            radialAxis.Value = radialAxis.ClampValue(radialAxis.Value);

            _view.OrbitalFollow.RadialAxis = radialAxis;
        }

        private bool CanUseTrackingTarget(Transform trackingTarget) {
            return trackingTarget != null &&
                   _view != null &&
                   _view.Camera != null &&
                   _view.OrbitalFollow != null &&
                   _view.TrackingTarget != null;
        }

        private void SetCameraTarget(Transform trackingTarget) {
            _view.Camera.Target.TrackingTarget = trackingTarget;
            _view.Camera.Target.LookAtTarget = trackingTarget;
        }
    }
}