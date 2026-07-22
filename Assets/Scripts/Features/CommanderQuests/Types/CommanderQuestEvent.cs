public readonly struct CommanderQuestEvent {
    public CommanderQuestEventType EventType { get; }
    public TeamType TeamType { get; }
    public CharacterType CharacterType { get; }
    public float Amount { get; }

    public bool IsValid => Amount > 0f &&
                          !float.IsNaN(Amount) &&
                          !float.IsInfinity(Amount);

    public CommanderQuestEvent(CommanderQuestEventType eventType,
                               TeamType teamType,
                               CharacterType characterType,
                               float amount = 1f) {
        EventType = eventType;
        TeamType = teamType;
        CharacterType = characterType;
        Amount = amount;
    }
}
