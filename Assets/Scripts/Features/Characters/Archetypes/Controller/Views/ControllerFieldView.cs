using UnityEngine;

public class ControllerFieldView : MonoBehaviour {

    private static readonly int BaseColorId =
        Shader.PropertyToID("_BaseColor");

    private static readonly int ColorId =
        Shader.PropertyToID("_Color");

    [SerializeField] private Renderer _renderer;

    private MaterialPropertyBlock _propertyBlock;

    private void Reset() {
        _renderer = GetComponentInChildren<Renderer>();
    }

    public void Show(Vector3 position,
                     float radius,
                     Color color) {

        transform.SetPositionAndRotation(
            position + Vector3.up * 0.03f,
            Quaternion.identity);

        transform.localScale =
            new Vector3(radius * 2f,
                        0.025f,
                        radius * 2f);

        ApplyColor(color);
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    private void ApplyColor(Color color) {
        if (_renderer == null) return;

        _propertyBlock ??= new MaterialPropertyBlock();
        _renderer.GetPropertyBlock(_propertyBlock);

        _propertyBlock.SetColor(BaseColorId, color);
        _propertyBlock.SetColor(ColorId, color);

        _renderer.SetPropertyBlock(_propertyBlock);
    }
}