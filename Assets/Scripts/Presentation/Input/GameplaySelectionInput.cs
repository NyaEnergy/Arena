using ConveyorWars.Presentation.Units;
using UnityEngine;

namespace ConveyorWars.Presentation.Input {
    public sealed class GameplaySelectionInput {
        private readonly GameplayInputRuntime _inputRuntime;
        private readonly GameplayControlConfig _config;
        private readonly Camera _camera;

        public GameplaySelectionInput(GameplayInputRuntime inputRuntime,
                                      GameplayControlConfig config,
                                      Camera camera) {
            _inputRuntime = inputRuntime;
            _config = config;
            _camera = camera;
        }

        public bool TryReadUnit(out UnitView unitView) {
            unitView = null;

            if (!_inputRuntime.WasLeaderSelectPressed()) {
                return false;
            }

            Vector2 pointerPosition = _inputRuntime.ReadPointerPosition();
            Ray ray = _camera.ScreenPointToRay(pointerPosition);

            if (!Physics.Raycast(ray,
                    out RaycastHit hit,
                    _config.MaxPointerRayDistance,
                    _config.UnitMask)) {
                return false;
            }

            unitView = hit.collider.GetComponentInParent<UnitView>();
            return unitView != null;
        }
    }
}
