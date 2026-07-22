public sealed class GameplaySceneSettings {
    public AllyCommanderConfig AlliedCommander { get; }
    public EnemyCommanderConfig EnemyCommander { get; }
    public StoryTerritoryConfig Territory { get; }

    public bool IsValid =>
        AlliedCommander != null &&
        AlliedCommander.IsValid &&
        AlliedCommander.TeamType == TeamType.Ally &&
        EnemyCommander != null &&
        EnemyCommander.IsValid &&
        EnemyCommander.TeamType == TeamType.Enemy &&
        Territory != null &&
        Territory.IsValid;

    public GameplaySceneSettings(AllyCommanderConfig alliedCommander,
                                 EnemyCommanderConfig enemyCommander,
                                 StoryTerritoryConfig territory) {
        AlliedCommander = alliedCommander;
        EnemyCommander = enemyCommander;
        Territory = territory;
    }
}
