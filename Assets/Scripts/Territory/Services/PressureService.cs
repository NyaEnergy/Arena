using Zenject;

public class PressureService : ITickable {
    private readonly TerritoryConfig _territoryConfig;
    private readonly TerritoryRuntime _territoryRuntime;

    private float _elapsedTime;

    public TerritoryRuntime Runtime => _territoryRuntime;

    public PressureService(TerritoryConfig territoryConfig) {
        _territoryConfig = territoryConfig;
        _territoryRuntime = new TerritoryRuntime();
    }

    public void Tick() {
        _elapsedTime += UnityEngine.Time.deltaTime;
        UpdatePressure();
    }

    private void UpdatePressure() {
        _territoryRuntime.SetPressure(_elapsedTime);

        if (_elapsedTime >= _territoryConfig.CriticalPressureTime) {
            _territoryRuntime.SetPressureLevel(PressureLevel.Critical);
            return;
        }
        if (_elapsedTime >= _territoryConfig.HighPressureTime) {
            _territoryRuntime.SetPressureLevel(PressureLevel.High);
            return;
        }
        if (_elapsedTime >= _territoryConfig.MediumPressureTime) {
            _territoryRuntime.SetPressureLevel(PressureLevel.Medium);
            return;
        }
        _territoryRuntime.SetPressureLevel(PressureLevel.Low);
    }
}
