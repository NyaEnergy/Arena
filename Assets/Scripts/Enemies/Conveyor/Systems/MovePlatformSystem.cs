using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformSystem : MonoBehaviour {
    [SerializeField] private float _moveDuration = 0.25f;

    private EnemyConveyorRuntime _runtime;

    public void Construct(EnemyConveyorRuntime runtime) {
        _runtime = runtime;
    }

    public void ShiftRightAndAdd(EnemyConveyorSlotRuntime newSlot) {
        StartCoroutine(ShiftCoroutine(newSlot));
    }

    private IEnumerator ShiftCoroutine(EnemyConveyorSlotRuntime newSlot) {
        var slots = new List<EnemyConveyorSlotRuntime>(_runtime.Slots);
        slots.Reverse();

        Vector3[] startPositions = new Vector3[slots.Count];
        Vector3[] targetPositions = new Vector3[slots.Count];

        for(int i = 0; i < slots.Count; ++i) {
            startPositions[i] = slots[i].Platform.transform.position;
            targetPositions[i] = startPositions[i] + Vector3.right;
        }

        float t = 0;
        while(t < 1f) {
            t += Time.fixedDeltaTime / _moveDuration;
            for (int i = 0; i < slots.Count; ++i) {
                slots[i].Platform.transform.position =
                    Vector3.Lerp(startPositions[i],
                                 targetPositions[i],
                                 t);
                slots[i].Enemy.transform.position = slots[i].Platform.transform.position + Vector3.up;
            }
            yield return new WaitForFixedUpdate();
        }

        for(int i = 0; i < slots.Count; ++i) {
            slots[i].Platform.transform.position = Vector3.zero;
        }

        _runtime.AddFirst(newSlot);

        newSlot.Platform.transform.position = Vector3.zero;
    }
}
