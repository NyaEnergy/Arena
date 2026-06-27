using UnityEngine;
using Zenject;

public class PoolReuseDebugService : IInitializable, ITickable {
    private readonly PoolReuseDebugSettings _settings;
    private readonly PoolReuseDebugCharacterService _characterService;
    private readonly PoolReuseDebugValidator _validator;
    private readonly PoolReuseDebugTracker _tracker;
    private readonly PoolReuseDebugPresenter _presenter;

    private PoolReuseDebugState _state;
    private BattlefieldCharacter _currentEnemy;

    private float _remainingTime;
    private int _completedRespawns;

    public PoolReuseDebugService(PoolReuseDebugSettings settings,
                                 PoolReuseDebugCharacterService characterService,
                                 PoolReuseDebugValidator validator,
                                 PoolReuseDebugTracker tracker,
                                 PoolReuseDebugPresenter presenter) {
        _settings = settings;
        _characterService = characterService;
        _validator = validator;
        _tracker = tracker;
        _presenter = presenter;
    }

    public void Initialize() {
        _state = PoolReuseDebugState.WaitingForCharacters;
        _presenter.ShowWaitingForCharacters();
    }

    public void Tick() {
        switch (_state) {
            case PoolReuseDebugState.WaitingForCharacters:
                TickWaitingForCharacters();
                break;
            case PoolReuseDebugState.WaitingBeforeKill:
                TickWaitingBeforeKill();
                break;
            case PoolReuseDebugState.WaitingForDespawn:
                TickWaitingForDespawn();
                break;
            case PoolReuseDebugState.WaitingBeforeRespawn:
                TickWaitingBeforeRespawn();
                break;
        }
    }

    private void TickWaitingForCharacters() {
        if (!_characterService.HasActiveAlly) {
            _presenter.ShowWaitingForCharacters();
            return;
        }

        if (!_characterService.TryGetActiveEnemy(out _currentEnemy)) {
            _presenter.ShowWaitingForCharacters();
            return;
        }

        _characterService.ResetActiveAlly();
        _tracker.Register(_currentEnemy);

        StartWaitingBeforeKill();
    }

    private void TickWaitingBeforeKill() {
        if (!_characterService.HasActiveEnemy) {
            StartWaitingBeforeRespawn();
            return;
        }

        _remainingTime -= Time.deltaTime;

        if (_remainingTime > 0f) return;

        _characterService.Eliminate(_currentEnemy);
        _remainingTime =_settings.DespawnTimeout;
        _state =PoolReuseDebugState.WaitingForDespawn;
        _presenter.ShowWaitingForDespawn(_completedRespawns);
    }

    private void TickWaitingForDespawn() {
        if (!_characterService.HasActiveEnemy) {
            StartWaitingBeforeRespawn();
            return;
        }

        _remainingTime -= Time.deltaTime;

        if (_remainingTime > 0f) return;

        Fail("Противник не покинул BattlefieldRegistry.");
    }

    private void TickWaitingBeforeRespawn() {
        if (!_characterService.HasActiveAlly) {
            Fail("На поле не осталось активного героя.");
            return;
        }

        _remainingTime -= Time.deltaTime;

        if (_remainingTime > 0f) return;

        SpawnAndValidateEnemy();
    }

    private void SpawnAndValidateEnemy() {
        _characterService.ResetActiveAlly();

        BattlefieldCharacter enemy = _characterService.SpawnEnemy();

        PoolReuseDebugValidationResult result = _validator.Validate(enemy);

        if (!result.IsValid) {
            Fail(result.Message);
            return;
        }

        _tracker.Register(enemy);
        _currentEnemy = enemy;
        _completedRespawns++;

        if (_completedRespawns >= _settings.RespawnCount) {
            Complete();
            return;
        }

        StartWaitingBeforeKill();
    }

    private void StartWaitingBeforeKill() {
        _remainingTime = _settings.KillDelay;
        _state = PoolReuseDebugState.WaitingBeforeKill;
        _presenter.ShowWaitingBeforeKill(_completedRespawns);
    }

    private void StartWaitingBeforeRespawn() {
        _remainingTime = _settings.RespawnDelay;
        _state = PoolReuseDebugState.WaitingBeforeRespawn;
        _presenter.ShowWaitingBeforeRespawn(_completedRespawns);
    }

    private void Complete() {
        if (!_tracker.HasReusedCharacter) {
            Fail("Pool не вернул ранее использованный объект.");
            return;
        }

        _state = PoolReuseDebugState.Completed;

        _presenter.ShowCompleted(_completedRespawns);
    }

    private void Fail(string message) {
        _state = PoolReuseDebugState.Failed;
        _presenter.ShowFailed(message, _completedRespawns);
    }
}