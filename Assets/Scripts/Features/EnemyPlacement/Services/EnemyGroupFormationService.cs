using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public sealed class EnemyGroupFormationService {
    private const float MINIMUM_SPACING = 0.1f;

    public bool TryCreate(TerritoryRuntime territory,
                          Vector3 dropPosition,
                          int count,
                          float requestedSpacing,
                          List<Vector3> positions) {
        positions?.Clear();

        if (territory?.View == null ||
            positions == null ||
            count <= 0) {
            return false;
        }

        float spacing = Mathf.Max(MINIMUM_SPACING,
                                  requestedSpacing);

        for (int index = 0; index < count; index++) {
            Vector3 candidate = dropPosition +
                GetOffset(index, count, spacing);

            if (!TrySample(territory,
                           candidate,
                           spacing,
                           positions,
                           out Vector3 position)) {
                positions.Clear();
                return false;
            }

            positions.Add(position);
        }

        return positions.Count == count;
    }

    private Vector3 GetOffset(int index,
                              int count,
                              float spacing) {
        if (count == 2) {
            float xOffset = index == 0 ?
                           -spacing * 0.5f :
                            spacing * 0.5f;

            return new Vector3(xOffset, 0f, 0f);
        }

        if (index == 0) return Vector3.zero;

        int ringCount = count - 1;
        int ringIndex = index - 1;

        float radius = spacing * Mathf.Max(
            1f, ringCount / (Mathf.PI * 2f));

        float angle = Mathf.PI * 2f * ringIndex / ringCount;

        return new Vector3(
            Mathf.Cos(angle) * radius,
            0f,
            Mathf.Sin(angle) * radius);
    }

    private bool TrySample(TerritoryRuntime territory,
                           Vector3 candidate,
                           float spacing,
                           IReadOnlyList<Vector3> existingPositions,
                           out Vector3 position) {
        position = default;

        float sampleDistance = Mathf.Max(0.35f, spacing * 0.6f);

        if (!NavMesh.SamplePosition(candidate,
                                out NavMeshHit hit,
                                    sampleDistance,
                                    NavMesh.AllAreas) ||
            !territory.View.Contains(hit.position)) {
            return false;
        }

        float minimumSqrDistance = spacing * spacing * 0.1f;

        for (int i = 0; i < existingPositions.Count; i++) {
            if (Vector3.SqrMagnitude(existingPositions[i] - hit.position) < minimumSqrDistance) {
                return false;
            }
        }

        position = hit.position;
        return true;
    }
}
