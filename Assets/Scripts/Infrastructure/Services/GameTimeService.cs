using UnityEngine;
using Zenject;
using R3;

public class GameTimeService : ITickable {
    private readonly ReactiveProperty<float> _deltaTime = new(0.016f);
    public ReadOnlyReactiveProperty<float> DeltaTine => _deltaTime;
    public void Tick() {
        _deltaTime.Value = Time.deltaTime;
    }
}
