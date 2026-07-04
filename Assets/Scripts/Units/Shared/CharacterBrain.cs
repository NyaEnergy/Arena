public class CharacterBrain {
    private readonly CharacterView _view;
    private readonly ICharacterConfig _config;
    private readonly CharacterRuntime _runtime;

    private readonly HealthComponent _healthComponent;
    private readonly MovementComponent _movementComponent;
    private readonly CombatComponent _combatComponent;
    private readonly TargetComponent _targetComponent;

    public CharacterView View => _view;
    public ICharacterConfig Config => _config;
    public CharacterRuntime Runtime => _runtime;
    public HealthComponent HealthComponent => _healthComponent;
    public MovementComponent MovementComponent => _movementComponent;
    public CombatComponent CombatComponent => _combatComponent;
    public TargetComponent TargetComponent => _targetComponent;

    public CharacterBrain(CharacterView view,
                          ICharacterConfig config,
                          TeamType teamType) {

        _view = view;
        _config = config;

        _runtime = new CharacterRuntime(teamType, config.MaxHP);
        _healthComponent = new HealthComponent(_runtime);
        _targetComponent = new TargetComponent();
        _movementComponent = new MovementComponent(view, config.MoveSpeed);

        ICharacterAttackConfig attackConfig =
            config as ICharacterAttackConfig;

        if (attackConfig != null) {
            _combatComponent = new CombatComponent(
                this, attackConfig, _targetComponent);
        }
    }

    public void Reset() {
        _runtime.Reset();
        _targetComponent.ClearTarget();
        _movementComponent.Reset();
        _combatComponent?.Reset();
    }
}