using UnityEngine;

public class SummonerMovementService {
    private const float DISTANCE_BUFFER = 0.2f;

    public bool Tick(CharacterBrain summoner,
                     CharacterBrain target,
                     SummonerConfig config) {
        float sqrDistance =
            Vector3.SqrMagnitude(
                summoner.View.transform.position -
                target.View.transform.position);

        float minimum =
            config.SummonDistanceRange.Min;

        float maximum =
            config.SummonDistanceRange.Max;

        if (sqrDistance > maximum * maximum) {
            summoner.MovementComponent
                .MoveToDistance(target.View.transform.position,
                                Mathf.Max(0f, maximum - DISTANCE_BUFFER),
                                1f);

            return false;
        }

        if (sqrDistance < minimum * minimum) {
            MoveAway(summoner,
                     target,
                     config.RetreatStepDistance);

            return false;
        }

        summoner.MovementComponent.Stop();

        return true;
    }

    private void MoveAway(CharacterBrain summoner,
                          CharacterBrain target,
                          float distance) {
        Vector3 current =
            summoner.View.transform.position;

        Vector3 direction =
            current - target.View.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) {
            summoner.MovementComponent.Stop();
            return;
        }

        summoner.MovementComponent.MoveToPosition(
            current + direction.normalized * distance);
    }
}