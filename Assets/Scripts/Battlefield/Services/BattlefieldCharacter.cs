using UnityEngine;
using Zenject;

public class BattlefieldCharacter : MonoBehaviour, IPoolableObject {
    [SerializeField] private CharacterInstaller _installer;

    private BattlefieldRegistry _battlefieldRegistry;
    private DetectionService _detectionService;
    private UtilityAIService _utilityAIService;
    private CharacterPool _characterPool;

    private CharacterAIController _aiController;

    private CharacterKey _characterKey;

    private bool _isInitialized;
    private bool _isSpawned;
    private bool _isRegistered;
    private bool _isReturnToPool;

    public CharacterView View => _installer.Brain.View;
    public CharacterBrain Brain => _installer.Brain;
    public CharacterKey CharacterKey => _characterKey;

    [Inject]
    private void Construct(
        BattlefieldRegistry battlefieldRegistry,
        DetectionService detectionService,
        UtilityAIService utilityAIService,
        CharacterPool characterPool) {
        _battlefieldRegistry = battlefieldRegistry;
        _detectionService = detectionService;
        _utilityAIService = utilityAIService;
        _characterPool = characterPool;
    }

    private void Update() {
        if (!_isSpawned ||
            !_isRegistered ||
            _isReturnToPool ||
            _aiController == null) return;
        
        _aiController.Tick();
        
        if (!Brain.Runtime.IsDead.CurrentValue) return;

        _isReturnToPool = true;

        _characterPool.Return(this);
    }

    public void Initialize(CharacterKey characterKey) {
        if (_isInitialized) return;
        _characterKey = characterKey;
        _isInitialized = true;
    }

    public void OnSpawned() {
        if (!_isInitialized || _isSpawned) return;

        _isReturnToPool = false;
        
        View.Enable();
        View.ResetAnimationState();

        Brain.Reset();
        
        _isSpawned = true;
    }

    public void OnDespawned() {
        LeaveBattlefield();
        _aiController?.Reset();
        View.Disable();
        _isSpawned = false;
        _isReturnToPool = false;
    }

    public void EnterBattlefield() {
        if (!_isSpawned || _isRegistered) return;

        _aiController ??= new CharacterAIController(Brain, _detectionService, _utilityAIService);
        _aiController.Reset();
        _battlefieldRegistry.Register(Brain);
        _isRegistered = true;
    }

    public void LeaveBattlefield() {
        if (_isRegistered) {
            _battlefieldRegistry.Unregister(Brain);
            _isRegistered = false;
        }
        _installer.Brain.TargetComponent.ClearTarget();
    }
}
