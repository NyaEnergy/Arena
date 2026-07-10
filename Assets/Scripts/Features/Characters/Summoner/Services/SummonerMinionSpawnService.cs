using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class SummonerMinionSpawnService {
    private const float MIN_DIRECTION_SQR_MAGNITUDE = 0.001f;

    private readonly SummonerConfig _config;
    private readonly LazyInject<CharacterFactory> _characterFactory;

    public SummonerMinionSpawnService(SummonerConfig config,
                                      LazyInject<CharacterFactory> characterFactory) {
        _config = config;
        _characterFactory = characterFactory;
    }

    public CharacterView SpawnMinion(CharacterBrain summoner,
                                     CharacterBrain target,
                                     int spawnIndex) {
        if (summoner == null ||
            target == null ||
            summoner.View == null ||
            target.View == null) return null;

        MinionConfig minionConfig = _config.MinionConfig;

        if (minionConfig == null) return null;

        CharacterFactory characterFactory = _characterFactory.Value;

        if (characterFactory == null) return null;

        if (!TryGetSpawnPosition(summoner,
                                 target,
                                 spawnIndex,
                                 out Vector3 spawnPosition)) return null;

        CharacterKey minionKey = new(
                summoner.Runtime.TeamType,
                minionConfig.CharacterType);

        CharacterView minion =
            characterFactory.Spawn(
                minionKey, spawnPosition);

        if (minion == null) return null;

        RotateToTarget(minion, target);

        return minion;
    }

    private bool TryGetSpawnPosition(CharacterBrain summoner,
                                     CharacterBrain target,
                                     int spawnIndex,
                                     out Vector3 spawnPosition) {
        Vector3 summonerPosition =
            summoner.View.transform.position;

        Vector3 direction =
            GetDirectionToTarget(summoner, target);

        Vector3 side = new(-direction.z, 0f, direction.x);

        int safeSpawnIndex = spawnIndex >= 0 ? spawnIndex : 0;

        float sideSign = safeSpawnIndex % 2 == 0 ? -1f : 1f;

        int sideStep = safeSpawnIndex / 2 + 1;

        Vector3 primaryPosition = summonerPosition +
                                  direction * _config.MinionForwardOffset +
                                  side * (_config.MinionSideOffset *
                                          sideSign * sideStep);

        if (TrySampleNavMesh(primaryPosition,
                         out spawnPosition)) return true;

        Vector3 forwardFallbackPosition = summonerPosition +
                                          direction * _config.MinionForwardOffset;

        if (TrySampleNavMesh(forwardFallbackPosition,
                         out spawnPosition)) return true;

        return TrySampleNavMesh(summonerPosition,
                            out spawnPosition);
    }

    private Vector3 GetDirectionToTarget(CharacterBrain summoner,
                                         CharacterBrain target) {
        Vector3 direction = target.View.transform.position -
                            summoner.View.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <
            MIN_DIRECTION_SQR_MAGNITUDE) {

            direction = summoner.View.transform.forward;
            direction.y = 0f;
        }

        if (direction.sqrMagnitude <
            MIN_DIRECTION_SQR_MAGNITUDE) return Vector3.forward;

        return direction.normalized;
    }

    private bool TrySampleNavMesh(Vector3 position,
                              out Vector3 sampledPosition) {

        sampledPosition = Vector3.zero;

        float sampleDistance =
            _config.MinionNavMeshSampleDistance;

        if (sampleDistance <= 0f) return false;

        if (!NavMesh.SamplePosition(position,
                                out NavMeshHit hit,
                                    sampleDistance,
                                    NavMesh.AllAreas)) return false;

        sampledPosition = hit.position;

        return true;
    }

    private void RotateToTarget(CharacterView minion,
                                CharacterBrain target) {

        Vector3 direction = target.View.transform.position -
                            minion.transform.position;

        direction.y = 0f;

        if (direction.sqrMagnitude <
            MIN_DIRECTION_SQR_MAGNITUDE) return;

        minion.transform.rotation =
            Quaternion.LookRotation(
                direction.normalized,
                Vector3.up);
    }
}