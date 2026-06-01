using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConveyorRuntime {
    private readonly List<EnemyConveyorSlot> _slots = new();

    public IReadOnlyList<EnemyConveyorSlot> Slots => _slots;
    public int Count => _slots.Count;

    public void Initialize(Vector3 startPosition, float spacing) {
        _slots.Clear();
        AddInitialSlot(startPosition);
    }

    public EnemyConveyorSlot AddSlot(Vector3 position) {
        var slot = new EnemyConveyorSlot(position);
        _slots.Add(slot);
        return slot;
    }

    public EnemyConveyorSlot GetSlot(int index) {
        return _slots[index];
    }

    private void AddInitialSlot(Vector3 startPosition) {
        _slots.Add(new EnemyConveyorSlot(startPosition));
    }
}
