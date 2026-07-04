using R3;

public class CharacterRuntime {
    private readonly ReactiveProperty<float> _currentHP;
    private readonly ReactiveProperty<bool> _isDead;

    private readonly TeamType _teamType;
    private readonly float _maxHP;

    public ReadOnlyReactiveProperty<float> CurrentHP => _currentHP;
    public ReadOnlyReactiveProperty<bool> IsDead => _isDead;
    public TeamType TeamType => _teamType;

    public CharacterRuntime(TeamType team, float maxHP) {
        _teamType = team;
        _maxHP = maxHP;

        _currentHP = new ReactiveProperty<float>(maxHP);
        _isDead = new ReactiveProperty<bool>(false);

        Reset();
    }

    public void Reset() {
        _currentHP.Value = _maxHP;
        _isDead.Value = false;
    }

    public void ApplyDamage(float damage) {
        if (_isDead.Value || damage <= 0f) return;

        _currentHP.Value -= damage;

        if (_currentHP.Value > 0f) return;

        _currentHP.Value = 0f;
        _isDead.Value = true;
    }

    public void ApplyHealing(float healing) {
        if (_isDead.Value ||
            healing <= 0f ||
            _currentHP.Value >= _maxHP) return;

        _currentHP.Value = System.Math.Min(_currentHP.Value + healing, _maxHP);
    }
}
