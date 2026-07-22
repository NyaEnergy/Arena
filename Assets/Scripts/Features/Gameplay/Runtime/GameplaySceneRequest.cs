public sealed class GameplaySceneRequest {
    private GameplaySceneSettings _settings;

    public bool IsPrepared => _settings != null && _settings.IsValid;

    public bool TryPrepare(AllyCommanderConfig alliedCommander,
                           EnemyCommanderConfig enemyCommander,
                           StoryTerritoryConfig territory) {
        GameplaySceneSettings settings = new(
            alliedCommander,
            enemyCommander,
            territory);

        if (!settings.IsValid) return false;

        _settings = settings;
        return true;
    }

    public bool TryGet(out GameplaySceneSettings settings) {
        settings = IsPrepared ? _settings : null;
        return settings != null;
    }

    public void Clear() {
        _settings = null;
    }
}
