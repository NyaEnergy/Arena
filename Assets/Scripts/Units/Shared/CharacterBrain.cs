public class CharacterBrain {
    private readonly CharacterView _view;
    private readonly CharacterConfig _config;
    private readonly CharacterRuntime _runtime;

    private readonly HealthComponent _healthComponent;
    private readonly MovementComponent _movementComponent;
    private readonly CombatComponent _combatComponent;
    private readonly TargetComponent _targetComponent;

    public CharacterView View => _view;
    public CharacterConfig Config => _config;
    public CharacterRuntime Runtime => _runtime;

    public HealthComponent HealthComponent => _healthComponent;
    public MovementComponent MovementComponent => _movementComponent;
    public CombatComponent CombatComponent => _combatComponent;
    public TargetComponent TargetComponent => _targetComponent;

    public CharacterBrain(CharacterView view, CharacterConfig config) {
        _view = view;
        _config = config;

        _runtime = new(config);

        _healthComponent = new HealthComponent(_runtime);
        _targetComponent = new TargetComponent();
        _movementComponent = new MovementComponent(view, config);
        _combatComponent = new CombatComponent(this, config, _targetComponent);
    }
}
