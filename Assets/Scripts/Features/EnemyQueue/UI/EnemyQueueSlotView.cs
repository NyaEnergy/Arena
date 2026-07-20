using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EnemyQueueSlotView : MonoBehaviour,
                                  IBeginDragHandler,
                                  IDragHandler,
                                  IEndDragHandler {
    [SerializeField] private GameObject _emptyState;
    [SerializeField] private Image _icon;
    [SerializeField] private TMP_Text _countText;

    private EnemyQueueItem _item;
    private bool _isDragging;

    public event Action<int, Vector2> DragStarted;
    public event Action<int, Vector2> DragMoved;
    public event Action<int, Vector2, bool> DragEnded;

    public int Index { get; private set; } = -1;

    public void SetIndex(int index) {
        Index = index;
    }

    public void Render(EnemyQueueItem item) {
        _item = item;
        _isDragging = false;

        Refresh();
    }

    public void SetDragging(bool isDragging) {
        _isDragging = isDragging &&
                      _item != null;

        Refresh();
    }

    public void OnBeginDrag(PointerEventData eventData) {
        if (_item?.Icon == null ||
            Index < 0) {
            return;
        }

        DragStarted?.Invoke(Index, eventData.position);
    }

    public void OnDrag(PointerEventData eventData) {
        if (!_isDragging) return;

        DragMoved?.Invoke(Index, eventData.position);
    }

    public void OnEndDrag(PointerEventData eventData) {
        if (!_isDragging) return;

        bool isOverUi = eventData.pointerCurrentRaycast.module
                        is GraphicRaycaster;

        DragEnded?.Invoke(Index, eventData.position,
                          isOverUi);
    }

    private void Refresh() {
        bool isOccupied = _item?.Icon != null;

        if (_emptyState != null) {
            _emptyState.SetActive(!isOccupied);
        }

        if (_icon != null) {
            _icon.sprite = isOccupied ? _item.Icon : null;
            _icon.enabled = isOccupied && !_isDragging;
        }

        if (_countText != null) {
            bool showCount = isOccupied &&
                             !_isDragging &&
                             _item.Count > 1;

            _countText.text = showCount ?
                              $"×{_item.Count}" : string.Empty;

            _countText.enabled = showCount;
        }
    }
}