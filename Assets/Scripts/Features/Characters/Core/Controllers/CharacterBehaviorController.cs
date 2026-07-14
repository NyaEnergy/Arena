using UnityEngine;

public class CharacterBehaviorController {
    private readonly CharacterBrain _brain;
    private readonly DetectionService _detectionService;
    private readonly UtilityAIService _utilityAIService;
    private readonly CharacterGroupService _groupService;
    private readonly ICharacterBehavior _behavior;

    private readonly CharacterStateMachine _stateMachine;

    private readonly AIContext _context = new();
    private readonly CharacterGroupRuntime _groupRuntime = new();

    public CharacterBehaviorController(CharacterBrain brain,
                                       DetectionService detectionService,
                                       UtilityAIService utilityAIService,
                                       CharacterGroupService groupService,
                                       ICharacterBehavior behavior) {
        _brain = brain;
        _detectionService = detectionService;
        _utilityAIService = utilityAIService;
        _groupService = groupService;
        _behavior = behavior;

        _stateMachine = new CharacterStateMachine(
                new IdleState(brain),
                new MoveState(brain),
                new AttackState(brain),
                new RetreatState(brain),
                new DeadState(brain)
        );

        Reset();
    }

    public void Reset() {
        _brain.TargetComponent.ClearTarget();
        _context.Reset();
        _groupRuntime.Reset();
        _behavior?.Reset();

        _stateMachine.Reset(CharacterStateType.Idle);
    }

    public void Tick() {
        if (_brain.Runtime.IsDead.CurrentValue) {
            _stateMachine.SetState(CharacterStateType.Dead);
            return;
        }

        if (_groupService.Tick(_brain, _groupRuntime))
            return;

        if (_behavior != null) {
            _behavior.Tick();
            return;
        }

        CharacterBrain target =
            _detectionService.FindClosestTarget(_brain);

        _brain.TargetComponent.SetTarget(target);

        UpdateContext(target);

        AIActionType action =
            _utilityAIService.Evaluate(_context);

        ProcessAction(action);
        _stateMachine.Tick();
    }

    private void UpdateContext(CharacterBrain target) {
        _context.Self = _brain;
        _context.CurrentTarget = target;

        _context.CurrentHpPercent =
            _brain.Runtime.CurrentHP.CurrentValue /
            _brain.Config.MaxHP;

        if (target == null) {
            _context.SqrDistanceToTarget =
                float.MaxValue;

            return;
        }

        _context.SqrDistanceToTarget =
            Vector3.SqrMagnitude(_brain.View.transform.position -
                                 target.View.transform.position);
    }

    private void ProcessAction(AIActionType action) {
        switch (action) {
            case AIActionType.Idle: _stateMachine.SetState(CharacterStateType.Idle); break;
            case AIActionType.Chase: _stateMachine.SetState(CharacterStateType.Move); break;
            case AIActionType.Attack: _stateMachine.SetState(CharacterStateType.Attack); break;
            case AIActionType.Retreat: _stateMachine.SetState(CharacterStateType.Retreat); break;
        }
    }
}