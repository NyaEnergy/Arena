using System;

public class CharacterLifecycleController : IDisposable {
    private readonly CharacterView _view;
    private readonly CharacterBrain _brain;
    private readonly CharacterBehaviorController _behaviorController;
    private readonly HealthBarRuntime _healthBarRuntime;
    private readonly CharacterBattlefieldPresenceController _presenceController;
    private readonly CharacterDeathPresentationService _deathService;
    private readonly Action<CharacterView> _deathHandler;
    private readonly Action<CharacterView> _returnToPool;
    private readonly CharacterDeathRuntime _deathRuntime = new();

    private bool _isSpawned;
    private bool _isExiting;
    private bool _isDying;
    private bool _isReturningToPool;

    public CharacterBrain Brain => _brain;

    public CharacterLifecycleController(CharacterView view,
            CharacterBrain brain,
            CharacterBehaviorController behaviorController,
            HealthBarRuntime healthBarRuntime,
            CharacterBattlefieldPresenceController presenceController,
            CharacterDeathPresentationService deathService,
            Action<CharacterView> deathHandler,
            Action<CharacterView> returnToPool) {

        _view = view;
        _brain = brain;
        _behaviorController = behaviorController;
        _healthBarRuntime = healthBarRuntime;
        _presenceController = presenceController;
        _deathService = deathService;
        _deathHandler = deathHandler;
        _returnToPool = returnToPool;
    }

    public void Tick() {
        if (!_isSpawned ||
            _isReturningToPool) {
            return;
        }

        _presenceController.Tick();

        if (_isExiting) {
            TickExit();
            return;
        }

        if (_isDying) {
            TickDeath();
            return;
        }

        if (!_presenceController.IsReady) {
            return;
        }

        _behaviorController.Tick();

        if (_brain.Runtime
                  .IsDead
                  .CurrentValue) {
            BeginDeath();
        }
    }

    public void OnSpawned() {
        if (_isSpawned) return;

        _isExiting = false;
        _isDying = false;
        _isReturningToPool = false;

        _view.Show();
        _view.ShowPresentation();
        _view.SetNavigationEnabled(true);

        _presenceController.PrepareSpawn();

        _isSpawned = true;
    }

    public void OnDespawned() {
        _presenceController.RemoveImmediately();

        _view.HidePresentation();

        _behaviorController.Reset();
        _deathRuntime.Reset();
        _brain.Reset();
        _view.ResetVisualState();

        _view.Hide();

        _isSpawned = false;
        _isExiting = false;
        _isDying = false;
        _isReturningToPool = false;
    }

    public void EnterBattlefield() {
        if (!_isSpawned) return;

        _behaviorController.Reset();
        _presenceController.BeginEnter();
    }

    public void EnterBattlefield(
        CharacterPresenceTransitionRequest request) {
        if (!_isSpawned) return;

        _behaviorController.Reset();
        _presenceController.BeginEnter(request);
    }

    public void ExitBattlefield() {
        if (!CanExit()) return;

        _isExiting = true;
        _presenceController.BeginExit();
    }

    public void ExitBattlefield(CharacterPresenceTransitionRequest request) {
        if (!CanExit()) return;

        _isExiting = true;
        _presenceController.BeginExit(request);
    }

    public void Dispose() {
        _presenceController.RemoveImmediately();
        _healthBarRuntime?.Dispose();
    }

    private void BeginDeath() {
        _isDying = true;

        _presenceController.RemoveImmediately();
        _deathHandler?.Invoke(_view);
        _deathService.Begin(_deathRuntime);
    }

    private void TickDeath() {
        if (!_deathService.Tick(_deathRuntime)) {
            return;
        }

        ReturnToPool();
    }

    private void TickExit() {
        if (!_presenceController.IsExitComplete) {
            return;
        }

        ReturnToPool();
    }

    private bool CanExit() {
        return _isSpawned &&
               !_isExiting &&
               !_isDying &&
               !_isReturningToPool &&
               _presenceController.IsReady;
    }

    private void ReturnToPool() {
        _isReturningToPool = true;
        _returnToPool?.Invoke(_view);
    }
}