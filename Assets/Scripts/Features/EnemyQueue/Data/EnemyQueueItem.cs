using UnityEngine;

public sealed class EnemyQueueItem {
    public EnemyGroupConfig GroupConfig { get; }
    public CharacterType CharacterType { get; }
    public SummonerConfig SummonerConfig { get; }
    public Sprite Icon { get; }
    public int Count { get; }
    public float FormationSpacing { get; }

    public bool IsSummoner => CharacterType == CharacterType.Summoner;

    public bool IsValid => GroupConfig != null &&
                           GroupConfig.IsValid &&
                           Icon != null &&
                           Count > 0;

    public EnemyQueueItem(EnemyGroupConfig groupConfig) {
        GroupConfig = groupConfig;
        CharacterType = groupConfig != null ?
                        groupConfig.CharacterType : default;

        SummonerConfig = groupConfig?.SummonerConfig;
        Icon = groupConfig?.Icon;
        Count = groupConfig?.Count ?? 0;
        FormationSpacing = groupConfig?.FormationSpacing ?? 0f;
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