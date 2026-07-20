using R3;
using UnityEngine;

public class HealthComponent {
    private readonly CharacterRuntime _runtime;

    public float LastDamageTime { get; private set; }

    public ReadOnlyReactiveProperty<float> CurrentHP => _runtime.CurrentHP;
    public ReadOnlyReactiveProperty<bool> IsDead => _runtime.IsDead;

    public HealthComponent(CharacterRuntime runtime) {
        _runtime = runtime;
        Reset();
    }

    public void Reset() {
        LastDamageTime = float.NegativeInfinity;
    }

    public void ApplyDamage(float damage) {
        if (damage > 0f &&
            !_runtime.IsDead.CurrentValue) {
            LastDamageTime = Time.time;
        }

        _runtime.ApplyDamage(damage);
    }

    public void ApplyHealing(float healing) {
        _runtime.ApplyHealing(healing);
    }
}
