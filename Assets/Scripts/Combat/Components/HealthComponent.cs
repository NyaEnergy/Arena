using R3;

public class HealthComponent {
    private readonly CharacterRuntime _runtime;

    public ReadOnlyReactiveProperty<float> CurrentHP => _runtime.CurrentHP;
    public ReadOnlyReactiveProperty<bool> IsDead => _runtime.IsDead;

    public HealthComponent(CharacterRuntime runtime) {
        _runtime = runtime;
    }

    public void ApplyDamage(float damage) {
        _runtime.ApplyDamage(damage);
    }
}
