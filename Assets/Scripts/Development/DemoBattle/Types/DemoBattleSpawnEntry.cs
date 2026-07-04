using UnityEngine;

[System.Serializable]
public class DemoBattleSpawnEntry {
    [SerializeField] private TeamType _teamType;
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private Transform _spawnPoint;

    public TeamType TeamType => _teamType;
    public CharacterType CharacterType => _characterType;
    public Transform SpawnPoint => _spawnPoint;
    public bool IsValid => _spawnPoint != null;
}