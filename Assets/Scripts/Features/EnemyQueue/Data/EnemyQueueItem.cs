using UnityEngine;

public sealed class EnemyQueueItem {
    public CharacterType CharacterType { get; }
    public SummonerConfig SummonerConfig { get; }
    public Sprite Icon { get; }

    public bool IsSummoner => CharacterType == CharacterType.Summoner;

    public bool IsValid {
        get {
            if (Icon == null) return false;

            if (IsSummoner)
                return SummonerConfig != null;

            return CharacterType != CharacterType.Summoner &&
                   CharacterType != CharacterType.EliteCommander;
        }
    }

    public EnemyQueueItem(CharacterType characterType,
                          SummonerConfig summonerConfig,
                          Sprite icon) {

        CharacterType = characterType;
        SummonerConfig = summonerConfig;
        Icon = icon;
    }

    public CharacterDeploymentRequest CreateRequest() {
        if (IsSummoner) {
            return CharacterDeploymentRequest.ForSummoner(
                TeamType.Enemy, SummonerConfig);
        }

        return CharacterDeploymentRequest.ForCharacter(
            TeamType.Enemy, CharacterType);
    }
}