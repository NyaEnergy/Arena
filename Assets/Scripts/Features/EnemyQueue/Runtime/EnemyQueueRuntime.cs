using System;

public class EnemyQueueRuntime {
    public const int Capacity = 4;
    
    private readonly EnemyQueueItem[] _items = new EnemyQueueItem[Capacity];

    public event Action Changed;

    public int Count { get; private set; }

    public bool IsEmpty => Count == 0;
    public bool IsFull => Count >= Capacity;

    public EnemyQueueItem GetAt(int index) {
        if (index < 0 || index >= Count)
            return null;

        return _items[index];
    }

    internal bool TryAdd(EnemyQueueItem item) {
        if (item == null ||
            !item.IsValid ||
            IsFull) {
            return false;
        }

        _items[Count] = item;
        Count++;

        Changed?.Invoke();
        return true;
    }

    internal bool TryPeek(out EnemyQueueItem item) {
        item = null;

        if (IsEmpty) return false;

        item = _items[0];
        return item != null;
    }

    internal bool RemoveFirst() {
        return RemoveAt(0);
    }

    internal bool RemoveAt(int index) {
        if (index < 0 || index >= Count)
            return false;

        for (int i = index + 1; i < Count; i++) {
            _items[i - 1] = _items[i];
        }

        Count--;
        _items[Count] = null;

        Changed?.Invoke();
        return true;
    }

    public void Clear() {
        for (int i = 0; i < Count; i++) {
            _items[i] = null;
        }

        Count = 0;
        Changed?.Invoke();
    }
}