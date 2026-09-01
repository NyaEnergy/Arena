using UnityEngine;

namespace ConveyorWars.Units.Movement {
    public sealed class UnitMovementCollisionResolver {
        private const int COLLISION_BUFFER_SIZE = 8;

        private readonly Transform _transform;
        private readonly Collider _collider;

        private readonly Collider[] _collisionBuffer =
            new Collider[COLLISION_BUFFER_SIZE];

        public UnitMovementCollisionResolver(
            Transform transform,
            Collider collider) {
            _transform = transform;
            _collider = collider;
        }

        public Vector3 Resolve(Vector3 desiredPosition) {
            Bounds bounds = _collider.bounds;
            Vector3 offset = desiredPosition - _transform.position;
            Vector3 queryCenter = bounds.center + offset;
            int unitMask = 1 << _collider.gameObject.layer;

            int count = Physics.OverlapBoxNonAlloc(
                queryCenter,
                bounds.extents,
                _collisionBuffer,
                Quaternion.identity,
                unitMask,
                QueryTriggerInteraction.Ignore);

            Vector3 resolvedPosition = desiredPosition;

            for (int i = 0; i < count; i++) {
                Collider other = _collisionBuffer[i];

                if (other == null ||
                    other == _collider ||
                    other.transform == _transform ||
                    other.transform.IsChildOf(_transform)) {
                    continue;
                }

                if (!Physics.ComputePenetration(
                        _collider,
                        resolvedPosition,
                        _transform.rotation,
                        other,
                        other.transform.position,
                        other.transform.rotation,
                        out Vector3 separationDirection,
                        out float separationDistance)) {
                    continue;
                }

                separationDirection.y = 0f;

                if (separationDirection.sqrMagnitude <= Mathf.Epsilon) {
                    continue;
                }

                resolvedPosition += separationDirection.normalized *
                                    separationDistance;

                resolvedPosition.y = desiredPosition.y;
            }

            return resolvedPosition;
        }
    }
}