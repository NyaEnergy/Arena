using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Conveyor Config", fileName = "EnemyConveyorConfig")]
public class EnemyConveyorConfig : ScriptableObject {
    [SerializeField] private int _maxQueueSize = 6;
    [SerializeField] private float _overflowSpawnDelay = 1.5f;
    [SerializeField] private float _directorSpawnInterval = 4f;

    public int MaxQueueSize => _maxQueueSize;
    public float OverflowSpawnDelay => _overflowSpawnDelay;
    public float DirectorSpawnInterval => _directorSpawnInterval;
}
