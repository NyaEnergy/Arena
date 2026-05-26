using UnityEngine;

public class CharacterSpawnerInstaller : MonoBehaviour {
    [SerializeField] private Transform _allySpawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;

    public Transform AllySpawnPoint => _allySpawnPoint;
    public Transform EnemySpawnPoint => _enemySpawnPoint;
}
