using R3;

public class CharacterRuntime {
    private readonly ReactiveProperty<float> _currentHP;
    private readonly ReactiveProperty<bool> _isDead;

    public ReadOnlyReactiveProperty<float> CurrentHP => _currentHP;
    public ReadOnlyReactiveProperty<bool> IsDead => _isDead;

    public CharacterRuntime(CharacterConfig config) {
        _currentHP = new ReactiveProperty<float>(config.MaxHP);
        _isDead = new ReactiveProperty<bool>(false);
    }

    public void ApplyDamage(float damage) {
        if (_isDead.Value) return;

        _currentHP.Value -= damage;

        if (_currentHP.Value <= 0f) {
            _currentHP.Value = 0f;
            _isDead.Value = true;
        }
    }
}
