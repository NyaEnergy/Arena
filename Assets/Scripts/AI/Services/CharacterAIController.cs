using UnityEngine;

public class CharacterAIController {
    private readonly CharacterBrain _brain;
    private readonly DetectionService _detectionService;
    private readonly CharacterStateMachine _stateMachine;

    public CharacterAIController(CharacterBrain brain, DetectionService detectionService) {
        _brain = brain;
        _detectionService = detectionService;

        _stateMachine = new CharacterStateMachine(
            new IdleState(brain),
            new MoveState(brain),
            new AttackState(brain),
            new DeadState(brain));

        _stateMachine.SetState(CharacterStateType.Idle);
    }

    public void Tick(CharacterBrain[] targets) {
        if (_brain.Runtime.IsDead.CurrentValue) {
            _stateMachine.SetState(CharacterStateType.Dead);
            return;
        }
        
        CharacterBrain target = _detectionService.FindClosestTarget(_brain, targets);
        _brain.TargetComponent.SetTarget(target);
        
        if(target == null) {
            _stateMachine.SetState(CharacterStateType.Idle);
            return;
        }
        
        float distance = Vector3.SqrMagnitude(_brain.View.transform.position - target.View.transform.position);
        
        if(distance <= _brain.Config.AttackRange * _brain.Config.AttackRange) {
            _stateMachine.SetState(CharacterStateType.Attack);
        } else {
            _stateMachine.SetState(CharacterStateType.Move);
        }

        _stateMachine.Tick();
    }
}
