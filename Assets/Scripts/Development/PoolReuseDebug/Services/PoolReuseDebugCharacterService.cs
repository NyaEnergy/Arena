public class PoolReuseDebugCharacterService {
    private readonly CharacterFactory _characterFactory;
    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly CharacterSpawnerInstaller _spawnerInstaller;

    public int ActiveAllyCount =>
        _battlefieldRegistry.Allies.Count;

    public int ActiveEnemyCount =>
        _battlefieldRegistry.Enemies.Count;

    public bool HasActiveAlly =>
        ActiveAllyCount > 0;

    public bool HasActiveEnemy =>
        ActiveEnemyCount > 0;

    public PoolReuseDebugCharacterService(CharacterFactory characterFactory,
                                          BattlefieldRegistry battlefieldRegistry,
                                          CharacterSpawnerInstaller spawnerInstaller) {
        _characterFactory = characterFactory;
        _battlefieldRegistry = battlefieldRegistry;
        _spawnerInstaller = spawnerInstaller;
    }

    public bool TryGetActiveEnemy(out BattlefieldCharacter character) {
        character = null;

        if (!HasActiveEnemy) return false;

        CharacterBrain brain = _battlefieldRegistry.Enemies[0];

        if (brain == null) return false;

        character = brain.View.GetComponentInParent<BattlefieldCharacter>();

        return character != null;
    }

    public void ResetActiveAlly() {
        if (!HasActiveAlly) return;
        _battlefieldRegistry.Allies[0].Reset();
    }

    public void Eliminate(BattlefieldCharacter character) {
        if (character == null) return;
        character.Brain.HealthComponent.ApplyDamage(float.MaxValue);
    }

    public BattlefieldCharacter SpawnEnemy() {
        CharacterKey enemyKey =
            new CharacterKey(TeamType.Enemy,
                             CharacterType.Vanguard);

        UnityEngine.Vector3 position = _spawnerInstaller.EnemySpawnPoint.position;
        return _characterFactory.Spawn(enemyKey, position);
    }
}
