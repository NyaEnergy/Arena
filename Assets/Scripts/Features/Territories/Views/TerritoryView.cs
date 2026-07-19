using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TerritoryView : MonoBehaviour {
    [SerializeField] private BoxCollider _bounds;

    public Bounds Bounds => _bounds.bounds;

    private void Reset() {
        _bounds = GetComponent<BoxCollider>();

        if (_bounds != null) {
            _bounds.isTrigger = true;
        }
    }

    private void OnValidate() {
        if (_bounds == null) {
            _bounds = GetComponent<BoxCollider>();
        }

        if (_bounds != null) {
            _bounds.isTrigger = true;
        }
    }

    public bool Contains(Vector3 position) {
        return _bounds != null &&
               _bounds.bounds.Contains(position);
    }
}