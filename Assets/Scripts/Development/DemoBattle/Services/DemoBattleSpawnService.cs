using System.Collections.Generic;
using Zenject;

public class DemoBattleSpawnService : IInitializable {
    private readonly CharacterFactory _characterFactory;
    private readonly DemoBattleView _view;

    public DemoBattleSpawnService(CharacterFactory characterFactory,
                                  DemoBattleView view) {
        _characterFactory = characterFactory;
        _view = view;
    }

    public void Initialize() {
        IReadOnlyList<DemoBattleSpawnEntry> spawnEntries = _view.SpawnEntries;

        for (int i = 0; i < spawnEntries.Count; i++) {
            DemoBattleSpawnEntry entry = spawnEntries[i];

            if (entry == null || !entry.IsValid) continue;

            Spawn(entry);
        }
    }

    private void Spawn(DemoBattleSpawnEntry entry) {

        CharacterKey characterKey = new(entry.TeamType, entry.CharacterType);

        CharacterView character = _characterFactory.Spawn(
                characterKey, entry.SpawnPoint.position);

        if (character == null) return;

        character.transform.rotation =
            entry.SpawnPoint.rotation;
    }
}