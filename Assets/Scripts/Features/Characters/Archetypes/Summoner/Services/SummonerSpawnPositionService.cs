using UnityEngine;
using UnityEngine.AI;

public class SummonerSpawnPositionService {
    public bool TryGet(CharacterBrain summoner,
                       CharacterBrain target,
                       SummonerConfig config,
                                  int spawnIndex,
                                  out Vector3 position) {
        position = Vector3.zero;

        if (summoner == null ||
            target == null ||
            config == null)
                return false;

        Vector3 origin = summoner.View.transform.position;
        Vector3 direction = target.View.transform.position - origin;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f) {
            direction = summoner.View.transform.forward;
            direction.y = 0f;
        }

        if (direction.sqrMagnitude < 0.001f) {
            direction = Vector3.forward;
        }

        direction.Normalize();

        Vector3 side =
            new(-direction.z, 0f, direction.x);

        int safeIndex =
            Mathf.Max(0, spawnIndex);

        float sideSign =
            safeIndex % 2 == 0 ?
            -1f : 1f;

        int sideStep =
            safeIndex / 2 + 1;

        Vector3 primary = origin +
                          direction *
                          config.SummonForwardOffset +
                          side *
                          config.SummonSideOffset *
                          sideSign *
                          sideStep;

        if (TrySample(primary, config, out position))
            return true;

        Vector3 forward = origin +
                          direction *
                          config.SummonForwardOffset;

        if (TrySample(forward, config, out position))
            return true;

        return TrySample(origin, config, out position);
    }

    private bool TrySample(Vector3 source,
                    SummonerConfig config,
                       out Vector3 position) {

        position = Vector3.zero;

        if (!NavMesh.SamplePosition(source,
                     out NavMeshHit hit,
                             config.NavMeshSampleDistance,
                            NavMesh.AllAreas)) {
            return false;
        }

        position = hit.position;

        return true;
    }
}