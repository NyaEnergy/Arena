using UnityEngine;

public class CharacterAIController {
    private readonly CharacterBrain _brain;
    private readonly DetectionService _detectionService;
    private readonly UtilityAIService _utilityAIService;
    private readonly CharacterStateMachine _stateMachine;

    private readonly AIContext _context;

    public CharacterAIController(CharacterBrain brain, DetectionService detectionService) {
        _brain = brain;
        _detectionService = detectionService;
        _utilityAIService = new UtilityAIService();

        _context = new AIContext();

        _stateMachine = new CharacterStateMachine(
            new IdleState(brain),
            new MoveState(brain),
            new AttackState(brain),
            new RetreatState(brain),
            new DeadState(brain));

        _stateMachine.SetState(CharacterStateType.Idle);
    }

    public void Tick() {
        if (_brain.Runtime.IsDead.CurrentValue) {
            _stateMachine.SetState(CharacterStateType.Dead);
            return;
        }
        
        CharacterBrain target = _detectionService.FindClosestTarget(_brain);
        _brain.TargetComponent.SetTarget(target);
        
        if(target == null) {
            _stateMachine.SetState(CharacterStateType.Idle);
            return;
        }

        UpdateContext();

        AIActionType action = _utilityAIService.Evaluate(_context);
        ProcessAction(action);

        _stateMachine.Tick();
    }

    private void UpdateContext() {
        CharacterBrain target = _detectionService.FindClosestTarget(_brain);
        _brain.TargetComponent.SetTarget(target);
        
        _context.Self = _brain;
        _context.CurrentTarget = target;

        _context.CurrentHpPercent = _brain.Runtime.CurrentHP.CurrentValue / _brain.Config.MaxHP;

        if(target == null) {
            _context.DistanceToTarget = 999f;
            return;
        }

        _context.DistanceToTarget = Vector3.SqrMagnitude(_brain.View.transform.position - target.View.transform.position);
    }

    private void ProcessAction(AIActionType action) {
        switch (action) {
            case AIActionType.Idle: { _stateMachine.SetState(CharacterStateType.Idle); break; }
            case AIActionType.Chase: { _stateMachine.SetState(CharacterStateType.Move); break; }
            case AIActionType.Attack: { _stateMachine.SetState(CharacterStateType.Attack); break; }
            case AIActionType.Retreat: { _stateMachine.SetState(CharacterStateType.Retreat); break; }
        }
    }
}
