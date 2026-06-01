using UnityEngine;

public class EnemyPlatformView : MonoBehaviour {
    [SerializeField] private Transform _enemyAnchor;
    [SerializeField] private EnemyHoverZone _pointer;
    [SerializeField] private float _moveSpeed = 12f;

    private Vector3 _targetPosition;

    public Transform EnemyAnchor => _enemyAnchor;
    public EnemyHoverZone Pointer => _pointer;

    private void Awake() {
        _targetPosition = transform.position;
    }

    private void FixedUpdate() {
        transform.position = Vector3.MoveTowards(
            transform.position,
            _targetPosition,
            _moveSpeed * Time.deltaTime);
    }

    public void MoveTo(Vector3 targetPosition) {
        _targetPosition = targetPosition;
    }

    public void SnapTo(Vector3 targetPosition) {
        _targetPosition = targetPosition;
        transform.position = targetPosition;
    }
}
