public abstract class CharacterState {
    private protected readonly CharacterBrain _brain;

    public abstract CharacterStateType StateType { get; }

    protected CharacterState(CharacterBrain brain) {
        _brain = brain;
    }

    public virtual void Enter() { }
    public virtual void Exit() { }
    public virtual void Tick() { }
}
