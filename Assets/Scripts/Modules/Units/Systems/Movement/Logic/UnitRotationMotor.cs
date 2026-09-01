using UnityEngine;

namespace ConveyorWars.Units.Movement {
    public sealed class UnitRotationMotor {
        private const float FACING_ANGLE_TOLERANCE = 1f;

        private readonly Transform _transform;
        private readonly UnitMovementSettings _settings;

        private Vector3 _facingDirection;

        public UnitRotationMotor(
            Transform transform,
            UnitMovementSettings settings) {
            _transform = transform;
            _settings = settings;
        }

        public bool TryFace(Vector3 position) {
            Vector3 direction = position - _transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude <= Mathf.Epsilon) {
                return false;
            }

            _facingDirection = direction.normalized;
            return true;
        }

        public bool IsFacing(Vector3 position) {
            Vector3 direction = position - _transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude <= Mathf.Epsilon) {
                return true;
            }

            float angle = Vector3.Angle(
                _transform.forward,
                direction);

            return angle <= FACING_ANGLE_TOLERANCE;
        }

        public void Tick(float deltaTime) {
            if (_facingDirection.sqrMagnitude <= Mathf.Epsilon) {
                return;
            }

            Quaternion targetRotation = Quaternion.LookRotation(
                _facingDirection,
                Vector3.up);

            _transform.rotation = Quaternion.RotateTowards(
                _transform.rotation,
                targetRotation,
                _settings.RotationSpeed * deltaTime);
        }
    }
}