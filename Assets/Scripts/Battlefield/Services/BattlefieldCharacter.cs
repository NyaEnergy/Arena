using UnityEngine;
using Zenject;

public class BattlefieldCharacter : MonoBehaviour, IPoolableObject {
    [SerializeField] private CharacterInstaller _installer;
    [SerializeField] private CharacterType _characterType;

    private BattlefieldRegistry _battlefieldRegistry;
    private DetectionService _detectionService;
    private CharacterPool _characterPool;

    private CharacterAIController _aiController;

    public CharacterType CharacterType => _characterType;

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
        if (_installer.Brain.Runtime.IsDead.CurrentValue) {
            _characterPool.Return(_characterType, this);
            return;
        }
        _aiController.Tick();
    }

    public void OnSpawned() {
        _installer.CreateBrain();
        _battlefieldRegistry.Register(_installer.Brain);
        _aiController = new CharacterAIController(_installer.Brain, _detectionService);
        _installer.Brain.View.Enable();
    }

    public void OnDespawned() {
        _battlefieldRegistry.Unregister(_installer.Brain);
        _installer.Brain.View.Disable();
    }
}
