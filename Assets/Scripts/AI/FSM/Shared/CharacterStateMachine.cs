using System.Collections.Generic;

public class CharacterStateMachine {
    private readonly Dictionary<CharacterStateType, CharacterState> _states;
    private CharacterState _currentState;
    
    public CharacterState CurrentState => _currentState;

    public CharacterStateMachine(
        IdleState idleState,
        MoveState moveState,
        AttackState attackState,
        RetreatState retreatState,
        DeadState deadState) {
        _states = new Dictionary<CharacterStateType, CharacterState> {
            [CharacterStateType.Idle] = idleState,
            [CharacterStateType.Move] = moveState,
            [CharacterStateType.Attack] = attackState,
            [CharacterStateType.Retreat] = retreatState,
            [CharacterStateType.Dead] = deadState,
        };
    }

    public void SetState(CharacterStateType stateType) {
        CharacterState newState = _states[stateType];

        if (_currentState == newState) return;
        
        _currentState?.Exit();
        _currentState = newState;
        _currentState.Enter();
    }

    public void Tick() {
        _currentState?.Tick();
    }
}
