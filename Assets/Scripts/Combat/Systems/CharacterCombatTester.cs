using System.Collections.Generic;
using UnityEngine;

public class CharacterCombatTester : MonoBehaviour {
    [SerializeField] private CharacterInstaller _ally;
    [SerializeField] private CharacterInstaller _enemy;

    private DetectionService _detectionService;

    private List<CharacterBrain> _allies;
    private List<CharacterBrain> _enemies;

    private void Awake() {
        _detectionService = new DetectionService();
        _allies = new List<CharacterBrain> { _ally.Brain };
        _enemies = new List<CharacterBrain> { _enemy.Brain };
    }

    private void Update() {
        UpdateBrains(_ally.Brain, _enemies);
        UpdateBrains(_enemy.Brain, _allies);
    }

    private void UpdateBrains(CharacterBrain brain, List<CharacterBrain> targets) {
        if (brain.Runtime.IsDead.CurrentValue) return;
        CharacterBrain target = _detectionService.FindClosestTarget(brain, targets);
        if (target == null) return;
        brain.TargetComponent.SetTarget(target);
        float distance = Vector3.SqrMagnitude(brain.View.transform.position - target.View.transform.position);
        if(distance > brain.Config.AttackRange * brain.Config.AttackRange) {
            brain.MovementComponent.MoveTo(target.View.transform.position);
        } else {
            brain.MovementComponent.Stop();
            if(brain.CombatComponent.CanAttack()) {
                brain.CombatComponent.Attack();
            }
        }
    }
}
