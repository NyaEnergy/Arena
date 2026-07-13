using UnityEngine;
using UnityEngine.AI;

public class CharacterNavMeshPathService {
    private readonly NavMeshPath _path = new();

    public bool HasCompletePath(Vector3 start,
                                Vector3 destination,
                                float maxPathLength) {
        
        if (!NavMesh.CalculatePath(start,
                                   destination,
                                   NavMesh.AllAreas,
                                   _path)) {
            return false;
        }

        if (_path.status != NavMeshPathStatus.PathComplete) {
            return false;
        }

        return maxPathLength <= 0f ||
               GetLength() <= maxPathLength;
    }

    private float GetLength() {
        float length = 0f;
        Vector3[] corners = _path.corners;

        for (int i = 1; i < corners.Length; i++) {
            length += Vector3.Distance(corners[i - 1],
                                       corners[i]);
        }

        return length;
    }
}