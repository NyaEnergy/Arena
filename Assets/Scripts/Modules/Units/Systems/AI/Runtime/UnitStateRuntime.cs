namespace ConveyorWars.Units.AI {
    public sealed class UnitStateRuntime {
        public UnitState Current { get; private set; }

        public UnitStateRuntime() {
            Current = UnitState.Idle;
        }

        public void Set(UnitState state) {
            Current = state;
        }
    }
}
