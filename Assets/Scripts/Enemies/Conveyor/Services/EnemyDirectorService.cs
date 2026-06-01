using UnityEngine;
using Zenject;

public class EnemyDirectorService : ITickable {
    private readonly EnemyConveyorRuntime _runtime;
    private readonly EnemyConveyorConfig _config;
    private readonly EnemyPlatformPool _platformPool;
    private readonly CharacterPool _characterPool;

    private float _timer;

    private Vector3 _startPosition = Vector3.zero;
    private float _spacing = 2.2f;

    public EnemyDirectorService(EnemyConveyorRuntime runtime,
                                EnemyConveyorConfig config,
                                EnemyPlatformPool platformPool,
                                CharacterPool characterPool) {
        _runtime = runtime;
        _config = config;
        _platformPool = platformPool;
        _characterPool = characterPool;

        _runtime.Initialize(_startPosition, _spacing);

        SpawnSlot(0);
    }

    public void Tick() {
        _timer += Time.deltaTime;

        if (_timer < _config.DirectorSpawnInterval) return;

        _timer = 0f;
        TrySpawnSlot();
    }

    private void TrySpawnSlot() {
        if (_runtime.Count < 8)
            SpawnSlot(_runtime.Count);
    }

    private void SpawnSlot(int index) {
        Vector3 position = _startPosition + Vector3.right * (index * _spacing);

        var slot = _runtime.AddSlot(position);

        var platform = _platformPool.Get();
        slot.AttachPlatform(platform);

        var enemy = _characterPool.Get(
            new CharacterKey(TeamType.Enemy, CharacterType.Vanguard),
            platform.EnemyAnchor.position);

        slot.AttachCharacter(enemy);
    }
}
