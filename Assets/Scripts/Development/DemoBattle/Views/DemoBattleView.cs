using System.Collections.Generic;
using UnityEngine;

public class DemoBattleView : MonoBehaviour {
    [SerializeField] private List<DemoBattleSpawnEntry> _spawnEntries = new();
    public IReadOnlyList<DemoBattleSpawnEntry> SpawnEntries => _spawnEntries;
}