using UnityEngine;
using Zenject;

public class BattlefieldCharacter : MonoBehaviour {
    [SerializeField] private CharacterInstaller _installer;

    private BattlefieldRegistry _battlefieldRegistry;
    private DetectionService _detectionService;

    private CharacterAIController _aiController;

    [Inject]
    private void Construct(
        BattlefieldRegistry battlefieldRegistry,
        DetectionService detectionService) {
        _battlefieldRegistry = battlefieldRegistry;
        _detectionService = detectionService;
    }

    private void Awake() {
        _battlefieldRegistry.Register(_installer.Brain);
        _aiController = new CharacterAIController(_installer.Brain, _detectionService);
    }

    private void Update() {
        _aiController.Tick();
    }

    private void OnDestroy() {
        _battlefieldRegistry.Unregister(_installer.Brain);
    }
}
