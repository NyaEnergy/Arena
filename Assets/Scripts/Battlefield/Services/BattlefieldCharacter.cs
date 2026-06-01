using UnityEngine;
using Zenject;

public class BattlefieldCharacter : MonoBehaviour, IPoolableObject {
    [SerializeField] private CharacterInstaller _installer;

    private BattlefieldRegistry _battlefieldRegistry;
    private DetectionService _detectionService;
    private CharacterKey _characterKey;
    private CharacterPool _characterPool;

    private CharacterAIController _aiController;
    private CharacterSpawnState _spawnState;

    [Inject]
    private void Construct(
        BattlefieldRegistry battlefieldRegistry,
        DetectionService detectionService,
        CharacterPool characterPool) {
        _battlefieldRegistry = battlefieldRegistry;
        _detectionService = detectionService;
        _characterPool = characterPool;
    }

    private void Update() {
        if (_spawnState != CharacterSpawnState.Battlefield) return;

        if (_installer.Brain.Runtime.IsDead.CurrentValue) {
            _characterPool.Return(_characterKey, this);
            return;
        }

        _aiController.Tick();
    }

    public void Initialize(CharacterKey characterKey, CharacterSpawnState spawnState) {
        _characterKey = characterKey;
        _spawnState = spawnState;
    }

    public void OnSpawned() {
        if (_spawnState == CharacterSpawnState.Battlefield) {
            _installer.Brain.View.Enable();
            return;
        }
        _installer.Brain.View.EnableConveyorMode();
    }

    public void OnDespawned() {
        _installer.Brain.View.Disable();
    }

    public void EnterBattlefield() {
        _spawnState = CharacterSpawnState.Battlefield;
        _battlefieldRegistry.Register(_installer.Brain);
        _aiController ??= new CharacterAIController(_installer.Brain, _detectionService);
    }

    public void LeaveBattlefield() {
        _battlefieldRegistry.Unregister(_installer.Brain);
        _installer.Brain.TargetComponent.ClearTarget();
    }
}
