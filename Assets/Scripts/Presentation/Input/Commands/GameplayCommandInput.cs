using ConveyorWars.Presentation.Units;
using UnityEngine;

namespace ConveyorWars.Presentation.Input {
    public sealed class GameplayCommandInput {
        private readonly GameplayInputRuntime _inputRuntime;
        private readonly GameplayControlConfig _config;
        private readonly Camera _camera;

        private bool _isMoveDragActive;

        public GameplayCommandInput(
            GameplayInputRuntime inputRuntime,
            GameplayControlConfig config,
            Camera camera) {
            _inputRuntime = inputRuntime;
            _config = config;
            _camera = camera;
        }

        public bool TryRead(
            out GameplayCommand command) {
            command = default;

            bool wasPressed = _inputRuntime.WasMovePressed();
            bool isHeld = _inputRuntime.IsMoveHeld();
            bool wasReleased = _inputRuntime.WasMoveReleased();

            if (wasPressed) return TryHandlePress(out command);
            if (!_isMoveDragActive) return false;
            if (!isHeld && !wasReleased) return false;

            return TryHandleMoveDrag(wasReleased, out command);
        }

        private bool TryHandlePress(out GameplayCommand command) {
            command = default;

            Ray ray = CreatePointerRay();

            if (TryReadUnit(ray,
                    out UnitView unitView)) {
                _isMoveDragActive = false;

                command = GameplayCommand.CreateUnitInteraction(unitView);

                return true;
            }

            if (!TryReadGround(ray,
                    out Vector3 groundPoint)) {
                _isMoveDragActive = false;
                return false;
            }

            _isMoveDragActive = true;

            command = GameplayCommand.CreateMove(groundPoint);

            return true;
        }

        private bool TryHandleMoveDrag(bool wasReleased,
                                   out GameplayCommand command) {
            command = default;

            Ray ray = CreatePointerRay();

            if (!TryReadGround(ray,
                    out Vector3 groundPoint)) {
                if (wasReleased) {
                    _isMoveDragActive = false;
                }

                return false;
            }

            command = GameplayCommand.CreateMove(groundPoint);

            if (wasReleased) {
                _isMoveDragActive = false;
            }

            return true;
        }

        private Ray CreatePointerRay() {
            Vector2 pointerPosition = _inputRuntime.ReadPointerPosition();
            return _camera.ScreenPointToRay(pointerPosition);
        }

        private bool TryReadUnit(Ray ray,
                             out UnitView unitView) {
            unitView = null;

            if (!Physics.Raycast(ray,
                    out RaycastHit hit,
                    _config.MaxPointerRayDistance,
                    _config.UnitMask)) {
                return false;
            }

            unitView = hit.collider.GetComponentInParent<UnitView>();

            return unitView != null;
        }

        private bool TryReadGround(Ray ray,
                               out Vector3 groundPoint) {
            groundPoint = default;

            if (!Physics.Raycast(ray,
                    out RaycastHit hit,
                    _config.MaxPointerRayDistance,
                    _config.GroundMask)) {
                return false;
            }

            groundPoint = hit.point;
            return true;
        }
    }
}