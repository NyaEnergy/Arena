using System.Collections.Generic;
using UnityEngine;

public class AllySquadView : MonoBehaviour {
    [SerializeField] private List<AllySquadSpawnEntry> _spawnEntries = new();
    public IReadOnlyList<AllySquadSpawnEntry> SpawnEntries => _spawnEntries;
}
