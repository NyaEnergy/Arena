using System;
using UnityEngine;
using UnityEngine.AI;

public class DemoBattleCallPositionService {
    private const float RANDOM_RADIUS = 1.25f;
    private const float SAMPLE_DISTANCE = 2.5f;

    private readonly Transform _allyPoint;
    private readonly Transform _enemyPoint;

    private readonly System.Random _random = new();

    public DemoBattleCallPositionService(
            Transform allyPoint,
            Transform enemyPoint) {
        
        _allyPoint = allyPoint;
        _enemyPoint = enemyPoint;
    }

    public Vector3 GetPosition( TeamType teamType) {
        Transform point = GetPoint(teamType);

        if (point == null) {
            return Vector3.zero;
        }

        Vector3 candidate = point.position +
                            GetRandomOffset();

        if (NavMesh.SamplePosition(candidate,
                               out NavMeshHit hit,
                                   SAMPLE_DISTANCE,
                                   NavMesh.AllAreas)) {

            return hit.position;
        }

        return point.position;
    }

    public Quaternion GetRotation(TeamType teamType) {
        Transform point = GetPoint(teamType);

        return point != null ?
            point.rotation : Quaternion.identity;
    }

    private Transform GetPoint(TeamType teamType) {
        return teamType == TeamType.Ally ?
               _allyPoint : _enemyPoint;
    }

    private Vector3 GetRandomOffset() {
        double angle = _random.NextDouble() *
                       Math.PI * 2.0;

        double distance = _random.NextDouble() *
                          RANDOM_RADIUS;

        return new Vector3((float)(Math.Cos(angle) * distance),
                            0f,
                            (float)(Math.Sin(angle) * distance)
        );
    }
}