using R3;

public class CharacterRuntime {
    private readonly ReactiveProperty<float> _currentHP;
    private readonly ReactiveProperty<bool> _isDead;
    private readonly TeamType _teamType;
    private readonly float _maxHP;

    public ReadOnlyReactiveProperty<float> CurrentHP => _currentHP;
    public ReadOnlyReactiveProperty<bool> IsDead => _isDead;
    public TeamType TeamType => _teamType;

    public CharacterRuntime(CharacterConfig config) {
        _currentHP = new ReactiveProperty<float>(config.MaxHP);
        _isDead = new ReactiveProperty<bool>(false);

        _teamType = config.TeamType;
        _maxHP = config.MaxHP;

        Reset();
    }

    public void Reset() {
        _currentHP.Value = _maxHP;
        _isDead.Value = false;
    }

    public void ApplyDamage(float damage) {
        if (_isDead.Value) return;
        if (damage <= 0f) return;

        _currentHP.Value -= damage;

        if (_currentHP.Value > 0f) return;
     
        _currentHP.Value = 0f;
        _isDead.Value = true;

    }
}
