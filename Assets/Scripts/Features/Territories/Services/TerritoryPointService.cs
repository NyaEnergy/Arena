using UnityEngine;
using UnityEngine.AI;

public class TerritoryPointService {
    private const int RANDOM_ATTEMPTS = 48;
    private const int GRID_SIZE = 5;

    private const float NAVMESH_SAMPLE_DISTANCE = 2f;
    private const float VIEWPORT_MARGIN_X = 0.08f;
    private const float VIEWPORT_MARGIN_Y = 0.12f;

    private readonly Camera _camera;

    public TerritoryPointService(Camera camera) {
        _camera = camera;
    }

    public bool TryGet(TerritoryRuntime territory,
                   out Vector3 position) {
        position = default;

        if (territory?.View == null ||
            _camera == null) {
            return false;
        }

        Bounds bounds = territory.View.Bounds;

        for (int i = 0; i < RANDOM_ATTEMPTS; i++) {
            Vector3 candidate = GetRandomPoint(bounds);

            if (TryValidate(territory,
                            candidate,
                        out position)) {
                return true;
            }
        }

        return TryGetFromGrid(territory,
                              bounds,
                              out position);
    }

    private bool TryGetFromGrid(TerritoryRuntime territory,
                                Bounds bounds,
                                out Vector3 position) {
        position = default;

        for (int x = 0; x < GRID_SIZE; x++) {
            for (int z = 0; z < GRID_SIZE; z++) {
                float xFactor = (x + 0.5f) / GRID_SIZE;
                float zFactor = (z + 0.5f) / GRID_SIZE;

                Vector3 candidate = new(
                        Mathf.Lerp(bounds.min.x,
                                   bounds.max.x,
                                   xFactor),
                        bounds.center.y,
                        Mathf.Lerp(bounds.min.z,
                                   bounds.max.z,
                                   zFactor));

                if (TryValidate(territory,
                                candidate,
                            out position)) {
                    return true;
                }
            }
        }

        return false;
    }

    private bool TryValidate(TerritoryRuntime territory,
                             Vector3 candidate,
                         out Vector3 position) {
        position = default;

        if (!NavMesh.SamplePosition(candidate,
                                out NavMeshHit hit,
                                    NAVMESH_SAMPLE_DISTANCE,
                                    NavMesh.AllAreas)) {
            return false;
        }

        if (!territory.View.Contains(hit.position)) {
            return false;
        }

        if (!IsVisible(hit.position)) {
            return false;
        }

        position = hit.position;
        return true;
    }

    private bool IsVisible(Vector3 position) {
        Vector3 viewport = _camera.WorldToViewportPoint(
                                position + Vector3.up * 0.5f);

        return viewport.z > 0f &&
               viewport.x >= VIEWPORT_MARGIN_X &&
               viewport.x <= 1f - VIEWPORT_MARGIN_X &&
               viewport.y >= VIEWPORT_MARGIN_Y &&
               viewport.y <= 1f - VIEWPORT_MARGIN_Y;
    }

    private Vector3 GetRandomPoint(Bounds bounds) {
        return new Vector3(Random.Range(bounds.min.x,
                                        bounds.max.x),
                           bounds.center.y,
                           Random.Range(bounds.min.z,
                                        bounds.max.z));
    }
}