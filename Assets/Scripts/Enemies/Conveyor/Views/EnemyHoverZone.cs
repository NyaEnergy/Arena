using UnityEngine;

public class EnemyHoverZone : MonoBehaviour {
    [SerializeField] private EnemyConveyorSlotRuntime _slot;

    private Collider _collider;
    
    public EnemyConveyorSlotRuntime Slot => _slot;

    private void Awake() {
        TryGetComponent(out _collider);
    }

    public void Construct(EnemyConveyorSlotRuntime slot) {
        _slot = slot;
    }

    public bool Contains(Collider target) {
        return _collider.bounds.Contains(target.transform.position);
    }
}
