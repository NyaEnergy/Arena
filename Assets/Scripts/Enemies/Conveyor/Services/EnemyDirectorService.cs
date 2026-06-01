using UnityEngine;
using Zenject;

public class EnemyDirectorService : ITickable {
    private readonly EnemyConveyorRoot _conveyorRoot;
    private readonly EnemyConveyorConfig _config;

    private readonly EnemyConveyorLayoutService _layoutService;
    private readonly EnemyConveyorRuntime _runtime;

    private readonly EnemyPlatformPool _platformPool;
    private readonly CharacterPool _characterPool;

    private float _spawnTimer;

    public EnemyDirectorService(EnemyConveyorRoot conveyorRoot,
                                EnemyConveyorConfig config,
                                EnemyConveyorLayoutService layoutService,
                                EnemyConveyorRuntime runtime,
                                EnemyPlatformPool platformPool,
                                CharacterPool characterPool) {
        _conveyorRoot = conveyorRoot;
        _config = config;

        _layoutService = layoutService;
        _runtime = runtime;

        _platformPool = platformPool;
        _characterPool = characterPool;
    }

    public void Tick() {
        _spawnTimer += Time.deltaTime;

        if (_spawnTimer < _config.DirectorSpawnInterval) return;

        _spawnTimer = 0f;
        TryCreateConveyorSlot();
    }

    private void TryCreateConveyorSlot() {
        if (_runtime.IsFull(_config.MaxQueueSize)) return;
        CharacterKey enemyKey = new CharacterKey(TeamType.Enemy, CharacterType.Vanguard);
        EnemyPlatformView platform = _platformPool.Get();
        platform.transform.position = _conveyorRoot.SpawnPoint.position;
        BattlefieldCharacter enemy = _characterPool.Get(enemyKey, platform.EnemyAnchor.position);
        enemy.Initialize(enemyKey, CharacterSpawnState.Conveyor);
        enemy.OnSpawned();
        enemy.transform.SetParent(platform.EnemyAnchor, true);
        EnemyConveyorSlotRuntime slot = new EnemyConveyorSlotRuntime(platform, enemy);
        _runtime.AddFirst(slot);
        _layoutService.RefreshLayout();
    }
}
