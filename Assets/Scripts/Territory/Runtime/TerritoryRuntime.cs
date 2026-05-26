using R3;

public class TerritoryRuntime {
    private readonly ReactiveProperty<float> _currentPressure;
    private readonly ReactiveProperty<PressureLevel> _pressureLevel;

    public ReadOnlyReactiveProperty<float> CurrentPressure => _currentPressure;
    public ReadOnlyReactiveProperty<PressureLevel> PressureLevel => _pressureLevel;

    public TerritoryRuntime() {
        _currentPressure = new ReactiveProperty<float>(0f);
        _pressureLevel = new ReactiveProperty<PressureLevel>(global::PressureLevel.Low);
    }

    public void SetPressure(float pressure) {
        _currentPressure.Value = pressure;
    }

    public void SetPressureLevel(
        PressureLevel pressureLevel) {
        _pressureLevel.Value = pressureLevel;
    }
}