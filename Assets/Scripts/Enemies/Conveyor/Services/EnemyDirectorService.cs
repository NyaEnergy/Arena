using Zenject;

public class EnemyDirectorService : ITickable {
    private readonly EnemyConveyorRuntime _runtime;
    private readonly EnemyConveyorConfig _config;

    private float _spawnTimer;

    public EnemyDirectorService(EnemyConveyorRuntime runtime,
                                EnemyConveyorConfig config) {
        _runtime = runtime;
        _config = config;
    }

    public void Tick() {
        _spawnTimer += UnityEngine.Time.deltaTime;

        if (_spawnTimer < _config.DirectorSpawnInterval) return;

        _spawnTimer = 0f;
        SpawnEnemy();
    }

    private void SpawnEnemy() {
        if (_runtime.IsFull(_config.MaxQueueSize)) return;

        CharacterKey key = new CharacterKey(TeamType.Enemy,
                                            CharacterType.Vanguard);

        EnemySpawnRequest request = new EnemySpawnRequest(key);
        _runtime.Add(request);
    }
}
