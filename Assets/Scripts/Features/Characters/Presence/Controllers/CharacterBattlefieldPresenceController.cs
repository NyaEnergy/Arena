using UnityEngine;

public class CharacterBattlefieldPresenceController {
    private readonly CharacterView _view;
    private readonly CharacterBrain _brain;
    private readonly BattlefieldRegistry _registry;

    private readonly CharacterPresenceTransitionService _transitionService;
    private readonly CharacterCombatPresenceService _combatService;
    private readonly CharacterPresenceTransitionRuntime _runtime = new();

    private CharacterPresenceTransitionDirection _direction;

    private bool _isRegistered;
    private bool _isTransitioning;
    private bool _isExitComplete;

    public bool IsReady => _isRegistered &&
                          !_isTransitioning;

    public bool IsExitComplete => _isExitComplete;

    public CharacterBattlefieldPresenceController(CharacterView view,
                                                  CharacterBrain brain,
                                                  BattlefieldRegistry registry,
                                                  CharacterPresenceTransitionService transitionService,
                                                  CharacterCombatPresenceService combatService) {
        _view = view;
        _brain = brain;
        _registry = registry;
        _transitionService = transitionService;
        _combatService = combatService;
    }

    public void PrepareSpawn() {
        _runtime.Reset();

        _isRegistered = false;
        _isTransitioning = false;
        _isExitComplete = false;

        _combatService.SetEnabled(_view, false);
    }

    public void BeginEnter() {
        BeginEnter(CreateDefaultRequest(
            CharacterPresenceTransitionDirection.Enter));
    }

    public void BeginEnter(CharacterPresenceTransitionRequest request) {
        if (_isRegistered ||
            _isTransitioning) return;

        BeginTransition(_brain.Config.EntryPresentation,
                        request);
    }

    public void BeginExit() {
        BeginExit(CreateDefaultRequest(
            CharacterPresenceTransitionDirection.Exit));
    }

    public void BeginExit(
        CharacterPresenceTransitionRequest request) {
        if (_isTransitioning ||
            _isExitComplete)
                return;

        RemoveRegistration();
        _isExitComplete = false;

        BeginTransition(_brain.Config.ExitPresentation,
                        request);
    }

    public void Tick() {
        if (!_isTransitioning) return;

        if (_transitionService.Tick(_view, _runtime)) {
            CompleteTransition();
        }
    }

    public void RemoveImmediately() {
        _transitionService.Cancel(_view, _runtime);

        RemoveRegistration();

        _view.SetNavigationEnabled(false);

        _isTransitioning = false;
        _isExitComplete = false;
        _runtime.Reset();
    }

    private void BeginTransition(CharacterPresencePresentationConfig config,
                                 CharacterPresenceTransitionRequest request) {

        _direction = request.Direction;

        bool isComplete =
            _transitionService.Begin(
                _view,
                config,
                _runtime,
                request
            );

        if (isComplete) {
            CompleteTransition();
            return;
        }

        _isTransitioning = true;
    }

    private void CompleteTransition() {
        _isTransitioning = false;

        if (_direction == CharacterPresenceTransitionDirection.Enter) {
            _combatService.SetEnabled(_view, true);
            _registry.Register(_brain);
            _isRegistered = true;
        } else {
            _isExitComplete = true;
        }
    }

    private void RemoveRegistration() {
        if (_isRegistered) {
            _registry.Unregister(_brain);
        }

        _combatService.SetEnabled(_view, false);

        _brain.TargetComponent.ClearTarget();
        _isRegistered = false;
    }

    private CharacterPresenceTransitionRequest CreateDefaultRequest(CharacterPresenceTransitionDirection direction) {
        Transform transform = _view.transform;

        return new CharacterPresenceTransitionRequest(
            direction,
            transform.position,
            transform.position,
            transform.rotation,
            transform.rotation
        );
    }
}