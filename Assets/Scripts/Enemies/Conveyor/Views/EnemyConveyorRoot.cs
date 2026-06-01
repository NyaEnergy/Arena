using UnityEngine;

public class EnemyConveyorRoot : MonoBehaviour {
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private Transform _rightDirection;
    [SerializeField] private float _platformSpacing = 2.5f;

    public Transform SpawnPoint => _spawnPoint;
    public Vector3 RightDirection => _rightDirection.right;
    public float PlatformSpacing => _platformSpacing;
}
