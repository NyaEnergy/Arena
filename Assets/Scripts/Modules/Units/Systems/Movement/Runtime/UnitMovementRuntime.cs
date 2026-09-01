using UnityEngine;

namespace ConveyorWars.Units.Movement {
    public sealed class UnitMovementRuntime {
        public UnitMovementState State { get; private set; }
        public Vector3 Destination { get; private set; }

        public UnitMovementRuntime() {
            State = UnitMovementState.Idle;
            Destination = Vector3.zero;
        }

        public void SetDestination(Vector3 destination) {
            Destination = destination;
            State = UnitMovementState.Moving;
        }

        public void Stop() {
            State = UnitMovementState.Idle;
        }
    }
}
