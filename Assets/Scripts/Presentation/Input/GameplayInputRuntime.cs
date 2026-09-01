using System;
using UnityEngine;

namespace ConveyorWars.Presentation.Input {
    public sealed class GameplayInputRuntime : IDisposable {
        private readonly ConveyorWarsInputActions _inputActions;

        public GameplayInputRuntime() {
            _inputActions = new ConveyorWarsInputActions();
        }

        public void Enable() {
            _inputActions.Gameplay.Enable();
        }

        public void Disable() {
            _inputActions.Gameplay.Disable();
        }
        public bool IsMoveHeld() {
            return _inputActions.Gameplay.MoveCommand.IsPressed();
        }

        public bool WasMoveReleased() {
            return _inputActions.Gameplay.MoveCommand.WasReleasedThisFrame();
        }

        public bool WasMovePressed() {
            return _inputActions.Gameplay.MoveCommand.WasPressedThisFrame();
        }

        public bool WasLeaderSelectPressed() {
            return _inputActions.Gameplay.LeaderSelect.WasPressedThisFrame();
        }

        public Vector2 ReadPointerPosition() {
            return _inputActions.Gameplay.PointerPosition.ReadValue<Vector2>();
        }

        public bool IsCameraRotateHeld() {
            return _inputActions.Gameplay.CameraRotateHold.IsPressed();
        }

        public Vector2 ReadPointerDelta() {
            return _inputActions.Gameplay.PointerDelta.ReadValue<Vector2>();
        }

        public float ReadCameraZoom() {
            return _inputActions.Gameplay.CameraZoom.ReadValue<float>();
        }

        public void Dispose() {
            _inputActions.Dispose();
        }
    }
}
