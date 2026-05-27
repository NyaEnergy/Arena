using System.Collections.Generic;
using UnityEngine;

public class EnemyConveyorView : MonoBehaviour {
    [SerializeField] private EnemySlotView _slotPrefab;

    [SerializeField] private Transform _container;

    private readonly List<EnemySlotView> _slots = new();

    public void Render(IReadOnlyList<EnemySpawnRequest> queue) {
        EnsureSlotCount(queue.Count);

        for (int i = 0; i < _slots.Count; i++) {
            bool active = i < queue.Count;
            _slots[i].gameObject.SetActive(active);
        }
    }

    private void EnsureSlotCount(int count) {
        while (_slots.Count < count) {
            EnemySlotView slot = Instantiate(_slotPrefab, _container);
            _slots.Add(slot);
        }
    }
}
