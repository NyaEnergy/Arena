public class HealthComponent {
    private readonly CharacterRuntime _runtime;

    public HealthComponent(CharacterRuntime runtime) {
        _runtime = runtime;
    }

    public void TakeDamage(float value) {
        _runtime.CurrentHP -= value;
        if(_runtime.CurrentHP <= 0) {
            _runtime.IsDead = true;
        }
    }

    public void Heal(float value) {
        _runtime.CurrentHP += value;
    }
}
