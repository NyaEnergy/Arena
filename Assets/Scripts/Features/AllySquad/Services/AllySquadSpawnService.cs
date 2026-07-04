using System.Collections.Generic;
using Zenject;

public class AllySquadSpawnService : IInitializable {
    private readonly CharacterFactory _characterFactory;
    private readonly AllySquadView _view;

    public AllySquadSpawnService(CharacterFactory characterFactory,
                                 AllySquadView view) {
        _characterFactory = characterFactory;
        _view = view;
    }

    public void Initialize() {
        IReadOnlyList<AllySquadSpawnEntry> spawnEntries = _view.SpawnEntries;

        for(int i = 0; i < spawnEntries.Count; ++i) {
            AllySquadSpawnEntry entry = spawnEntries[i];
            
            if (entry == null || !entry.IsValid) continue;

            CharacterKey characterKey = new(TeamType.Ally,
                                            entry.CharacterType);

            _characterFactory.Spawn(characterKey,
                                    entry.SpawnPoint.position);
        }
    }
}
