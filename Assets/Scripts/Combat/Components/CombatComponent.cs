public class CombatComponent {
    private readonly CharacterConfig _config;
    private readonly HealthComponent _health;

    public CombatComponent(CharacterConfig config, HealthComponent health) {
        _config = config;
        _health = health;
    }

    public void Attack(HealthComponent target) {
        target.TakeDamage(_config.Damage);
    }
}
