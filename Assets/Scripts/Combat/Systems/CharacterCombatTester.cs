using UnityEngine;

public class CharacterCombatTester : MonoBehaviour {
    [SerializeField] private CharacterInstaller _ally;
    [SerializeField] private CharacterInstaller _enemy;

    private CharacterAIController _allyAI;
    private CharacterAIController _enemyAI;

    private void Awake() {
        DetectionService _detectionService = new DetectionService();
        _allyAI = new CharacterAIController(_ally.Brain, _detectionService);
        _enemyAI = new CharacterAIController(_enemy.Brain, _detectionService);
    }

    private void Update() {
        _allyAI.Tick(new[] { _enemy.Brain });
        _enemyAI.Tick(new[] { _ally.Brain });
    }
}
