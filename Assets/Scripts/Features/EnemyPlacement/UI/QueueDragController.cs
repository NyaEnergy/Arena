using System;
using UnityEngine;
using Zenject;

public class QueueDragController : IInitializable,
                                   IDisposable,
                                   IQueueDragState {
    private readonly EnemyPlacementService _placementService;
    private readonly EnemyQueueRuntime _runtime;
    private readonly EnemyQueueService _queueService;
    private readonly EnemyQueueView _queueView;
    private readonly QueueDragView _dragView;

    private EnemyQueueItem _draggedItem;
    private int _draggedIndex = -1;

    public bool IsDragging => _draggedItem != null &&
                              _draggedIndex >= 0;

    public QueueDragController(EnemyPlacementService placementService,
                               EnemyQueueRuntime runtime,
                               EnemyQueueService queueService,
                               EnemyQueueView queueView,
                               QueueDragView dragView) {

        _placementService = placementService;
        _runtime = runtime;
        _queueService = queueService;
        _queueView = queueView;
        _dragView = dragView;
    }

    public void Initialize() {
        for (int i = 0; i < _queueView.Slots.Count; i++) {
            EnemyQueueSlotView slot = _queueView.Slots[i];

            if (slot == null) continue;

            slot.DragStarted += Begin;
            slot.DragMoved += Move;
            slot.DragEnded += End;
        }
    }

    public void Dispose() {
        for (int i = 0; i < _queueView.Slots.Count; i++) {
            EnemyQueueSlotView slot = _queueView.Slots[i];

            if (slot == null) continue;

            slot.DragStarted -= Begin;
            slot.DragMoved -= Move;
            slot.DragEnded -= End;
        }

        Cancel();
    }

    private void Begin(int index, Vector2 screenPosition) {
        if (IsDragging) Cancel();

        EnemyQueueItem item = _runtime.GetAt(index);

        if (item?.Icon == null ||
            !_dragView.Show(
                item.Icon,
                item.Count,
                screenPosition)) {
            return;
        }

        _draggedItem = item;
        _draggedIndex = index;

        _queueView.SetDragging(index, true);
    }

    private void Move(int index, Vector2 screenPosition) {
        if (!IsDragging ||
            index != _draggedIndex) {
            return;
        }

        _dragView.Move(screenPosition);
    }

    private void End(int index,
                     Vector2 screenPosition,
                     bool isOverUi) {
        if (!IsDragging ||
            index != _draggedIndex) {
            Cancel();
            return;
        }

        if (!isOverUi &&
            _placementService.TryPlace(
                _draggedItem, screenPosition)) {
            _queueService.RemoveAt(_draggedIndex);
        }

        Cancel();
    }

    private void Cancel() {
        int index = _draggedIndex;

        _draggedItem = null;
        _draggedIndex = -1;

        _dragView.Hide();

        if (index >= 0) {
            _queueView.SetDragging(index, false);
        }
    }
}