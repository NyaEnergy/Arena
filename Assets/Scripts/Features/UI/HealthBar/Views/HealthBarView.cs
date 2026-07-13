using UnityEngine;
using UnityEngine.UI;

public class HealthBarView : MonoBehaviour {
    [SerializeField] private Transform _billboardRoot;
    [SerializeField] private Image _backgroundImage;
    [SerializeField] private Image _fillImage;

    private Transform _cameraTransform;

    private void LateUpdate() {
        if (_billboardRoot == null ||
            _cameraTransform == null)
            return;

        _billboardRoot.rotation = _cameraTransform.rotation;
    }

    public void Initialize(Transform cameraTransform,
                           Color backgroundColor,
                           Color fillColor) {
        _cameraTransform = cameraTransform;

        if (_backgroundImage != null) _backgroundImage.color = backgroundColor;
        if (_fillImage != null) _fillImage.color = fillColor;

        SetNormalizedValue(1f);
    }

    public void SetNormalizedValue(float normalizedValue) {
        if (_fillImage == null) return;
        Vector3 localScale = _fillImage.rectTransform.localScale;
        localScale.x = Mathf.Clamp01(normalizedValue);
        _fillImage.rectTransform.localScale = localScale;
    }
}
