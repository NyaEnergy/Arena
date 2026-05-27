using Zenject;

public class EnemyConveyorService : ITickable {
    private readonly EnemyConveyorRuntime _runtime;
    private readonly EnemyConveyorConfig _config;

    private readonly CharacterFactory _characterFactory;
    private readonly CharacterSpawnerInstaller _spawnerInstaller;

    private float _overflowTimer;

    public EnemyConveyorService(EnemyConveyorRuntime runtime,
                                EnemyConveyorConfig config,
                                CharacterFactory characterFactory,
                                CharacterSpawnerInstaller spawnerInstaller) {
        _runtime = runtime;
        _config = config;

        _characterFactory = characterFactory;
        _spawnerInstaller = spawnerInstaller;
    }

    public void Tick() {
        if (!_runtime.HasEnemies()) {
            _overflowTimer = 0f;
            return;
        }

        if (!_runtime.IsFull(_config.MaxQueueSize)) {
            _overflowTimer = 0f;
            return;
        }

        _overflowTimer += UnityEngine.Time.deltaTime;

        if (_overflowTimer < _config.OverflowSpawnDelay) return;

        _overflowTimer = 0f;
        SpawnFrontEnemy();
    }

    private void SpawnFrontEnemy() {
        EnemySpawnRequest request = _runtime.PopFront();

        _characterFactory.Spawn(request.CharacterKey,
                                _spawnerInstaller.EnemySpawnPoint.position);
    }
}
