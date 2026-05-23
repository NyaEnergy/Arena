using R3;

public class CharacterRuntime {
    private readonly ReactiveProperty<float> _currentHP;
    private readonly ReactiveProperty<bool> _isDead;
    private readonly TeamType _teamType;

    public ReadOnlyReactiveProperty<float> CurrentHP => _currentHP;
    public ReadOnlyReactiveProperty<bool> IsDead => _isDead;
    public TeamType TeamType => _teamType;

    public CharacterRuntime(CharacterConfig config) {
        _currentHP = new ReactiveProperty<float>(config.MaxHP);
        _isDead = new ReactiveProperty<bool>(false);
        _teamType = config.TeamType;
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
