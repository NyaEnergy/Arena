using UnityEngine;

public class PrimitiveCharacterPresentationView : MonoBehaviour {
    [SerializeField] private Transform _visualRoot;
    private GameObject _primitiveObject;

    public void Apply(PrimitiveType primitiveType,
                      Vector3 localPosition,
                      Vector3 localScale,
                      Material material) {
        Clear();

        _primitiveObject = GameObject.CreatePrimitive(primitiveType);
        _primitiveObject.name = "PrimitiveModel";
        _primitiveObject.layer = gameObject.layer;

        Transform primitiveTransform = _primitiveObject.transform;

        primitiveTransform.SetParent(_visualRoot, false);
        primitiveTransform.localPosition = localPosition;
        primitiveTransform.localRotation = Quaternion.identity;
        primitiveTransform.localScale = localScale;

        Collider primitiveCollider = primitiveTransform.GetComponent<Collider>();

        if (primitiveCollider != null) {
            primitiveCollider.enabled = false;
            Destroy(primitiveCollider);
        }

        Renderer primitiveRenderer = primitiveTransform.GetComponent<Renderer>();

        if (primitiveRenderer != null) {
            primitiveRenderer.sharedMaterial = material;
        }
    }

    private void Clear() {
        if (_primitiveObject == null) return;
        _primitiveObject.SetActive(false);
        Destroy(_primitiveObject);
        _primitiveObject = null;
    }
}
