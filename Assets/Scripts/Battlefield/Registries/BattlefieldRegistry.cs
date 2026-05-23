using System.Collections.Generic;
using UnityEngine;

public class BattlefieldRegistry {
    private readonly List<CharacterBrain> _allies = new();
    private readonly List<CharacterBrain> _enemies = new();

    public IReadOnlyList<CharacterBrain> Allies => _allies;
    public IReadOnlyList<CharacterBrain> Enemies => _enemies;

    public void Register(CharacterBrain brain) {
        if (brain.Runtime.TeamType == TeamType.Ally) {
            if (_allies.Contains(brain)) return;
            _allies.Add(brain);
            return;
        }
        if (_enemies.Contains(brain)) return;
        _enemies.Add(brain);
    }

    public void Unregister(CharacterBrain brain) {
        if(brain.Runtime.TeamType == TeamType.Ally) {
            _allies.Remove(brain);
            return;
        }
        _enemies.Remove(brain);
    }

    public IReadOnlyList<CharacterBrain> GetEnemies(TeamType teamType) {
        return teamType == TeamType.Ally ? _enemies : _allies;
    }
}
