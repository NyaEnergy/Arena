using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TerritoryDropService {
    private const float RAY_EPSILON = 0.0001f;

    private readonly Camera _camera;
    private readonly TerritoryRegistry _registry;

    public TerritoryDropService(Camera camera,
                                TerritoryRegistry registry) {
        _camera = camera;
        _registry = registry;
    }

    public bool TryGet(Vector2 screenPosition,
                   out TerritoryRuntime territory,
                   out Vector3 position) {
        territory = null;
        position = default;

        if (_camera == null ||
            !_camera.pixelRect.Contains(screenPosition)) {
            return false;
        }

        Ray ray = _camera.ScreenPointToRay(screenPosition);

        NavMeshTriangulation triangulation =
            NavMesh.CalculateTriangulation();

        Vector3[] vertices = triangulation.vertices;
        int[] indices = triangulation.indices;

        float nearestDistance = float.PositiveInfinity;

        for (int i = 0; i + 2 < indices.Length; i += 3) {
            Vector3 first = vertices[indices[i]];
            Vector3 second = vertices[indices[i + 1]];
            Vector3 third = vertices[indices[i + 2]];

            if (!TryIntersect(ray,
                              first,
                              second,
                              third,
                          out float distance)) {
                continue;
            }

            if (distance >= nearestDistance) continue;

            Vector3 candidate = ray.GetPoint(distance);

            TerritoryRuntime candidateTerritory =
                FindTerritory(candidate);

            if (candidateTerritory == null) continue;

            nearestDistance = distance;
            territory = candidateTerritory;
            position = candidate;
        }

        return territory != null;
    }

    private TerritoryRuntime FindTerritory(Vector3 position) {
        IReadOnlyList<TerritoryRuntime> territories =
            _registry.Territories;

        for (int i = 0; i < territories.Count; i++) {
            TerritoryRuntime territory = territories[i];

            if (territory?.View != null &&
                territory.View.Contains(position)) {
                return territory;
            }
        }

        return null;
    }

    private bool TryIntersect(Ray ray,
                              Vector3 first,
                              Vector3 second,
                              Vector3 third,
                              out float distance) {
        distance = 0f;

        Vector3 firstEdge = second - first;
        Vector3 secondEdge = third - first;
        Vector3 cross = Vector3.Cross(ray.direction,
                                      secondEdge);

        float determinant = Vector3.Dot(firstEdge, cross);

        if (Mathf.Abs(determinant) < RAY_EPSILON) return false;

        float inverse = 1f / determinant;
        Vector3 originOffset = ray.origin - first;

        float firstFactor = inverse *
                            Vector3.Dot(originOffset, cross);

        if (firstFactor < 0f || firstFactor > 1f) return false;

        Vector3 secondCross = Vector3.Cross(originOffset, firstEdge);

        float secondFactor = inverse *
                             Vector3.Dot(ray.direction, secondCross);

        if (secondFactor < 0f ||
            firstFactor + secondFactor > 1f) {
            return false;
        }

        distance = inverse * Vector3.Dot(secondEdge, secondCross);
        return distance > RAY_EPSILON;
    }
}