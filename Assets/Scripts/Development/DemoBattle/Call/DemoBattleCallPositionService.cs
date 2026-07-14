using UnityEngine;
using UnityEngine.AI;

public class DemoBattleCallPositionService {
    private const int ATTEMPT_COUNT = 64;

    private const float MIN_VIEWPORT_Y = 0.16f;
    private const float MAX_VIEWPORT_Y = 0.84f;

    private const float ALLY_MIN_X = 0.10f;
    private const float ALLY_MAX_X = 0.45f;

    private const float ENEMY_MIN_X = 0.55f;
    private const float ENEMY_MAX_X = 0.90f;

    private readonly Camera _camera;

    public DemoBattleCallPositionService( Camera camera) {
        _camera = camera;
    }

    public bool TryGetPosition(TeamType teamType,
                           out Vector3 position) {
        position = default;

        if (_camera == null) return false;

        NavMeshTriangulation triangulation =
            NavMesh.CalculateTriangulation();

        if (triangulation.indices == null ||
            triangulation.indices.Length < 3)
            return false;

        for (int i = 0; i < ATTEMPT_COUNT; i++) {
            Vector3 candidate = GetRandomPoint(triangulation);

            if (!IsAllowed(candidate, teamType)) continue;

            position = candidate;
            return true;
        }

        return TryGetFallback(triangulation,
                              teamType,
                          out position);
    }

    public Quaternion GetRotation(TeamType teamType) {
        if (_camera == null) return Quaternion.identity;

        Vector3 direction = Vector3.ProjectOnPlane(
                _camera.transform.right,
                Vector3.up
        );

        if (direction.sqrMagnitude < 0.001f)
            direction = Vector3.forward;

        if (teamType == TeamType.Enemy)
            direction = -direction;

        return Quaternion.LookRotation(
            direction.normalized,
            Vector3.up
        );
    }

    private bool TryGetFallback(NavMeshTriangulation triangulation,
        TeamType teamType, out Vector3 position) {
        position = default;

        float targetX = teamType == TeamType.Ally ?
                        0.3f : 0.7f;

        float bestScore = float.MaxValue;

        bool found = false;

        for (int i = 0; i < triangulation.indices.Length; i += 3) {
            Vector3 candidate = GetCenter(triangulation, i);

            Vector3 viewport = _camera.WorldToViewportPoint(
                               candidate + Vector3.up * 0.5f);

            if (!IsVisible(viewport)) continue;

            float score = Mathf.Abs(viewport.x - targetX) +
                          Mathf.Abs(viewport.y - 0.5f);

            if (score >= bestScore) continue;

            bestScore = score;
            position = candidate;
            found = true;
        }

        return found;
    }

    private bool IsAllowed(Vector3 position,
                           TeamType teamType) {

        Vector3 viewport = _camera.WorldToViewportPoint(
                           position + Vector3.up * 0.5f);

        if (!IsVisible(viewport)) return false;

        float minimumX = teamType == TeamType.Ally ?
            ALLY_MIN_X : ENEMY_MIN_X;

        float maximumX = teamType == TeamType.Ally ?
            ALLY_MAX_X : ENEMY_MAX_X;

        return viewport.x >= minimumX &&
               viewport.x <= maximumX;
    }

    private bool IsVisible(Vector3 viewport) {
        return viewport.z > 0f &&
               viewport.x >= 0f &&
               viewport.x <= 1f &&
               viewport.y >= MIN_VIEWPORT_Y &&
               viewport.y <= MAX_VIEWPORT_Y;
    }

    private Vector3 GetRandomPoint(NavMeshTriangulation triangulation) {
        int triangleCount = triangulation.indices.Length / 3;

        int index = Random.Range(0, triangleCount) * 3;

        Vector3 first = triangulation.vertices[
                        triangulation.indices[index]];

        Vector3 second = triangulation.vertices[
                         triangulation.indices[index + 1]];

        Vector3 third = triangulation.vertices[
                        triangulation.indices[index + 2]];

        float firstWeight = Mathf.Sqrt(Random.value);

        float secondWeight = Random.value;

        return (1f - firstWeight) * first +
               firstWeight *
               (1f - secondWeight) * second +
               firstWeight *
               secondWeight * third;
    }

    private Vector3 GetCenter(NavMeshTriangulation triangulation,
                              int index) {
        Vector3 first = triangulation.vertices[
                        triangulation.indices[index]];

        Vector3 second = triangulation.vertices[
                         triangulation.indices[index + 1]];

        Vector3 third = triangulation.vertices[
                        triangulation.indices[index + 2]];

        return (first + second + third) / 3f;
    }
}