public readonly struct CharacterKey {
    public readonly TeamType TeamType;
    public readonly CharacterType CharacterType;

    public CharacterKey(TeamType teamType,
                        CharacterType characterType) {
        TeamType = teamType;
        CharacterType = characterType;
    }
}
