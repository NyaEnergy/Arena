using UnityEngine;
using UnityEngine.UI;

public class QueueDragView : MonoBehaviour {
    [SerializeField] private Canvas _canvas;
    [SerializeField] private RectTransform _root;
    [SerializeField] private Image _icon;

    public bool Show(Sprite sprite,
                     Vector2 screenPosition) {
        if (_root == null ||
            _icon == null ||
             sprite == null) {
                return false;
        }

        _root.gameObject.SetActive(true);

        _icon.raycastTarget = false;
        _icon.sprite = sprite;

        Move(screenPosition);

        return true;
    }

    public void Move(Vector2 screenPosition) {
        if (_canvas == null ||
            _root == null) {
            return;
        }

        RectTransform parent = _root.parent as RectTransform;

        if (parent == null) return;

        Camera camera = _canvas.renderMode == RenderMode.ScreenSpaceOverlay ?
                        null : _canvas.worldCamera;

        if (RectTransformUtility
            .ScreenPointToLocalPointInRectangle(
                parent, screenPosition, camera,
                out Vector2 localPosition)) {

            _root.anchoredPosition = localPosition;
        }
    }

    public void Hide() {
        if (_root != null) {
            _root.gameObject.SetActive(false);
        }

        if (_icon != null) {
            _icon.sprite = null;
        }
    }
}