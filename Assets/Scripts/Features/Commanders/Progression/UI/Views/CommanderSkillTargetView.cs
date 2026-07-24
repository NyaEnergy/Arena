using UnityEngine;

public sealed class CommanderSkillTargetView : MonoBehaviour {
    private const float MINIMUM_RADIUS = 0.75f;

    [SerializeField] private GameObject _root;
    [SerializeField] private LineRenderer _circle;
    [SerializeField, Min(12)] private int _segments = 48;
    [SerializeField, Min(0f)] private float _heightOffset = 0.08f;
    [SerializeField, Min(0.01f)] private float _width = 0.08f;
    [SerializeField] private Color _validColor = new(0.2f, 0.9f, 1f, 0.9f);
    [SerializeField] private Color _invalidColor = new(1f, 0.2f, 0.2f, 0.9f);

    private void Awake() {
        Hide();
    }

    public void Show(Vector3 position,
                     float requestedRadius,
                     bool isValid) {

        if (_root == null ||
            _circle == null) return;

        _root.SetActive(true);

        int segmentCount = Mathf.Max(12, _segments);
        float radius = Mathf.Max(MINIMUM_RADIUS,
                                 requestedRadius);
        float width = Mathf.Max(0.01f, _width);
        Color color = isValid ? _validColor : _invalidColor;

        _circle.useWorldSpace = true;
        _circle.loop = true;
        _circle.positionCount = segmentCount;
        _circle.startWidth = width;
        _circle.endWidth = width;
        _circle.startColor = color;
        _circle.endColor = color;

        for (int i = 0; i < segmentCount; ++i) {
            float angle = Mathf.PI * 2f * i / segmentCount;

            Vector3 point = position + new Vector3(
                Mathf.Cos(angle) * radius,
                _heightOffset,
                Mathf.Sin(angle) * radius);

            _circle.SetPosition(i, point);
        }
    }

    public void Hide() {
        if (_root != null) {
            _root.SetActive(false);
        }
    }
}
