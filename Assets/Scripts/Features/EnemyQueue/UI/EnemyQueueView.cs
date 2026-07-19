using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyQueueView : MonoBehaviour {
    [SerializeField] private Button _releaseButton;
    [SerializeField] private List<EnemyQueueSlotView> _slots = new();

    public Button ReleaseButton => _releaseButton;
    public IReadOnlyList<EnemyQueueSlotView> Slots => _slots;

    public void Render(EnemyQueueRuntime runtime) {
        if (runtime == null) return;

        int visibleCount = Mathf.Min(runtime.Count, _slots.Count);
        int firstSlotIndex = _slots.Count - visibleCount;

        for (int slotIndex = 0; slotIndex < _slots.Count; slotIndex++) {
            EnemyQueueSlotView slot = _slots[slotIndex];

            if (slot == null) continue;

            int queueIndex = slotIndex - firstSlotIndex;
            EnemyQueueItem item = queueIndex >= 0 ?
                                  runtime.GetAt(queueIndex) : null;

            slot.SetIndex(queueIndex);
            slot.Render(item);
        }

        if (_releaseButton != null) {
            _releaseButton.interactable = !runtime.IsEmpty;
        }
    }

    public void SetDragging(int queueIndex, bool isDragging) {
        for (int i = 0; i < _slots.Count; i++) {
            EnemyQueueSlotView slot = _slots[i];

            if (slot == null ||
                slot.Index != queueIndex) {
                continue;
            }

            slot.SetDragging(isDragging);
            return;
        }
    }
}