namespace ConveyorWars.Units {
    public sealed class UnitEntity {
        public UnitSide Side { get; }

        public UnitEntity(UnitSide side) {
            Side = side;
        }
    }
}
