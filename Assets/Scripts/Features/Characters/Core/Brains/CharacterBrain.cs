public class CharacterBrain {
    private readonly CharacterView _view;
    private readonly ICharacterRuntimeConfig _config;
    private readonly CharacterRuntime _runtime;

    private readonly HealthComponent _healthComponent;
    private readonly MovementComponent _movementComponent;
    private readonly CombatComponent _combatComponent;
    private readonly TargetComponent _targetComponent;

    public CharacterView View => _view;
    public ICharacterRuntimeConfig Config => _config;
    public CharacterRuntime Runtime => _runtime;
    public HealthComponent HealthComponent => _healthComponent;
    public MovementComponent MovementComponent => _movementComponent;
    public CombatComponent CombatComponent => _combatComponent;
    public TargetComponent TargetComponent => _targetComponent;

    public CharacterBrain(CharacterView view,
                          ICharacterRuntimeConfig config,
                          TeamType teamType,
                          EnemyDirectorService directorService) {
        _view = view;
        _config = config;

        _runtime = new CharacterRuntime(teamType, config.MaxHP);
        _healthComponent = new HealthComponent(_runtime);
        _targetComponent = new TargetComponent();
        _movementComponent = new MovementComponent(view, config.MoveSpeed);

        if (config is ICharacterAttackConfig attackConfig) {
            _combatComponent = new CombatComponent(this,
                                                   attackConfig,
                                                  _targetComponent,
                                                   directorService);
        }
    }

    public void Reset() {
        _runtime.Reset();
        _healthComponent.Reset();
        _targetComponent.ClearTarget();
        _movementComponent.Reset();
        _combatComponent?.Reset();
    }
}