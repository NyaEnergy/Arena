using UnityEngine;

namespace ConveyorWars.Units.Movement {
    public sealed class UnitMovementMotor {
        private readonly Transform _transform;
        private readonly UnitMovementSettings _settings;
        private readonly UnitMovementRuntime _runtime;
        private readonly UnitRotationMotor _rotationMotor;
        private readonly UnitMovementCollisionResolver _collisionResolver;

        public UnitMovementState State => _runtime.State;
        public Vector3 Destination => _runtime.Destination;

        public UnitMovementMotor(Transform transform,
                                 Collider collider,
                                 UnitMovementSettings settings,
                                 UnitMovementRuntime runtime) {
            _transform = transform;
            _settings = settings;
            _runtime = runtime;

            _rotationMotor = new UnitRotationMotor(transform, settings);
            _collisionResolver = new UnitMovementCollisionResolver(transform, collider);
        }

        public bool TrySetDestination(Vector3 destination) {
            destination.y = _transform.position.y;

            _runtime.SetDestination(destination);
            _rotationMotor.TryFace(destination);

            return true;
        }

        public bool TryFace(Vector3 position) {
            return _rotationMotor.TryFace(position);
        }

        public bool IsFacing(Vector3 position) {
            return _rotationMotor.IsFacing(position);
        }

        public void Stop() {
            _runtime.Stop();
        }

        public void Tick(float deltaTime) {
            if (deltaTime <= 0f) return;

            if (_runtime.State == UnitMovementState.Moving) {
                _rotationMotor.TryFace(_runtime.Destination);
            }

            _rotationMotor.Tick(deltaTime);

            if (_runtime.State == UnitMovementState.Moving) {
                Move(deltaTime);
            }
        }

        private void Move(float deltaTime) {
            Vector3 toDestination = _runtime.Destination - _transform.position;

            toDestination.y = 0f;

            float stoppingDistance = _settings.StoppingDistance;

            if (toDestination.sqrMagnitude <= stoppingDistance * stoppingDistance) {
                _runtime.Stop();
                return;
            }

            Vector3 desiredPosition =
                Vector3.MoveTowards(
                    _transform.position,
                    _runtime.Destination,
                    _settings.MoveSpeed * deltaTime);

            desiredPosition.y = _transform.position.y;

            _transform.position = _collisionResolver.Resolve(desiredPosition);
        }
    }
}