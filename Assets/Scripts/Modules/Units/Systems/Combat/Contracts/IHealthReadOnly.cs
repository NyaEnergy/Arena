namespace ConveyorWars.Units.Combat {
    public interface IHealthReadOnly {
        int CurrentHealth { get; }
        int MaxHealth { get; }
        bool IsAlive { get; }
    }
}
