using UnityEngine;
using Zenject;

public class CharacterBootstrap : IInitializable {
    private readonly CharacterFactory _characterFactory;

    private readonly CharacterSpawnerInstaller _spawnerInstaller;

    public CharacterBootstrap(CharacterFactory characterFactory,
                              CharacterSpawnerInstaller spawnerInstaller) {
        _characterFactory = characterFactory;
        _spawnerInstaller = spawnerInstaller;
    }

    public void Initialize() {
        SpawnInitialCharacters();
    }

    private void SpawnInitialCharacters() {
        CharacterKey allyKey = new CharacterKey(TeamType.Ally, CharacterType.Vanguard);
        CharacterKey enemyKey = new CharacterKey(TeamType.Enemy, CharacterType.Vanguard);
        _characterFactory.Spawn(allyKey, _spawnerInstaller.AllySpawnPoint.position);
        _characterFactory.Spawn(enemyKey, _spawnerInstaller.EnemySpawnPoint.position);
    }
}
