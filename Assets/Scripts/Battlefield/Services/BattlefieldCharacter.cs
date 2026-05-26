using UnityEngine;
using Zenject;

public class BattlefieldCharacter : MonoBehaviour, IPoolableObject {
    [SerializeField] private CharacterInstaller _installer;

    private BattlefieldRegistry _battlefieldRegistry;
    private DetectionService _detectionService;
    private CharacterKey _characterKey;
    private CharacterPool _characterPool;

    private CharacterAIController _aiController;

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
            _characterPool.Return(_characterKey, this);
            return;
        }
        _aiController.Tick();
    }

    public void Initialize(CharacterKey characterKey) {
        _characterKey = characterKey;
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
