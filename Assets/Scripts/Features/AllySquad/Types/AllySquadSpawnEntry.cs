using UnityEngine;

[System.Serializable]
public class AllySquadSpawnEntry {
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private Transform _spawnPoint;

    public CharacterType CharacterType => _characterType;
    public Transform SpawnPoint => _spawnPoint;

    public bool IsValid => _spawnPoint != null;
}
