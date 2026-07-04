using System;

public class CharacterController : IDisposable {
    private readonly CharacterView _view;
    private readonly CharacterKey _characterKey;
    private readonly CharacterBrain _brain;

    private readonly CharacterAIController _aiController;
    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly HealthBarRuntime _healthBarRuntime;

    private readonly Action<CharacterView> _returnToPool;

    private bool _isSpawned;
    private bool _isRegistered;
    private bool _isReturningToPool;

    public CharacterKey CharacterKey => _characterKey;
    public CharacterBrain Brain => _brain;

    public CharacterController(CharacterView view,
                               CharacterKey characterKey,
                               CharacterBrain brain,
                               CharacterAIController aiController,
                               BattlefieldRegistry battlefieldRegistry,
                               HealthBarRuntime healthBarRuntime,
                               Action<CharacterView> returnToPool) {
        _view = view;
        _characterKey = characterKey;
        _brain = brain;
        _aiController = aiController;
        _battlefieldRegistry = battlefieldRegistry;
        _healthBarRuntime = healthBarRuntime;
        _returnToPool = returnToPool;
    }

    public void Tick() {
        if (!_isSpawned ||
            !_isRegistered ||
            _isReturningToPool) return;

        _aiController.Tick();

        if (!_brain.Runtime.IsDead.CurrentValue) return;

        _isReturningToPool = true;
        _returnToPool?.Invoke(_view);
    }

    public void OnSpawned() {
        if (_isSpawned) return;

        _isReturningToPool = false;

        _view.Show();
        _view.ResetAnimationState();

        _brain.Reset();
        
        _isSpawned = true;
    }

    public void OnDespawned() {
        LeaveBattlefield();

        _aiController.Reset();
        _view.Hide();

        _isSpawned = false;
        _isReturningToPool = false;
    }

    public void EnterBattlefield() {
        if (!_isSpawned || _isRegistered) return;

        _aiController.Reset();

        _battlefieldRegistry.Register(_brain);

        _isRegistered = true;
    }

    public void LeaveBattlefield() {
        if (_isRegistered) {
            _battlefieldRegistry.Unregister(_brain);
            _isRegistered = false;
        }

        _brain.TargetComponent.ClearTarget();
    }

    public void Dispose() {
        LeaveBattlefield();
        _healthBarRuntime?.Dispose();
    }
}
