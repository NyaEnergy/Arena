using System.Collections.Generic;

public class EnemyConveyorRuntime {
    private readonly List<EnemyConveyorSlotRuntime> _slots = new();

    public IReadOnlyList<EnemyConveyorSlotRuntime> Slots => _slots;
    public int Count => _slots.Count;

    public bool IsFull(int maxCount) {
        return _slots.Count >= maxCount;
    }

    public void AddFirst(EnemyConveyorSlotRuntime slot) {
        _slots.Insert(0, slot);
    }

    public EnemyConveyorSlotRuntime RemoveLast() {
        int lastIndex = _slots.Count - 1;
        var slot = _slots[lastIndex];
        _slots.RemoveAt(lastIndex);
        return slot;
    }

    public EnemyConveyorSlotRuntime Get(int index) {
        return _slots[index];
    }
}
