public sealed class CharacterDeploymentRequest {
    public TeamType TeamType { get; }
    public CharacterType CharacterType { get; }
    public SummonerConfig SummonerConfig { get; }

    public bool IsSummoner =>
        CharacterType == CharacterType.Summoner;

    public bool IsValid {
        get {
            if (IsSummoner) return SummonerConfig != null;

            return CharacterType != CharacterType.Summoner &&
                   CharacterType != CharacterType.EliteCommander;
        }
    }

    private CharacterDeploymentRequest(TeamType teamType,
                                       CharacterType characterType,
                                       SummonerConfig summonerConfig) {
        TeamType = teamType;
        CharacterType = characterType;
        SummonerConfig = summonerConfig;
    }

    public static CharacterDeploymentRequest ForCharacter(TeamType teamType,
                                                          CharacterType characterType) {
        return new CharacterDeploymentRequest(
            teamType, characterType, null);
    }

    public static CharacterDeploymentRequest ForSummoner(
        TeamType teamType, SummonerConfig summonerConfig) {
        
        return new CharacterDeploymentRequest(
            teamType, CharacterType.Summoner, summonerConfig);
    }
}