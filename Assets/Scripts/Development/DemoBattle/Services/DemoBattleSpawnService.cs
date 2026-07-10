using System.Collections.Generic;
using Zenject;

public class DemoBattleSpawnService : IInitializable {
    private readonly IReadOnlyList<DemoBattleSpawnEntry> _spawnEntries;
    private readonly CharacterFactory _characterFactory;

    public DemoBattleSpawnService(IReadOnlyList<DemoBattleSpawnEntry> spawnEntries,
                                  CharacterFactory characterFactory) {
        _spawnEntries = spawnEntries;
        _characterFactory = characterFactory;
    }

    public void Initialize() {
        for (int i = 0; i < _spawnEntries.Count; i++) {
            DemoBattleSpawnEntry spawnEntry = _spawnEntries[i];

            if (!spawnEntry.IsValid) continue;

            CharacterKey characterKey =
                new(spawnEntry.TeamType, spawnEntry.CharacterType);

            CharacterView character =
                _characterFactory.Spawn(
                    characterKey, spawnEntry.SpawnPoint.position);

            if (character == null) continue;

            character.transform.rotation =
                spawnEntry.SpawnPoint.rotation;
        }
    }
}