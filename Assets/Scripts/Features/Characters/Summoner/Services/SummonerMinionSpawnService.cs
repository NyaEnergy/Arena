using UnityEngine;
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
            target == null) {

            return null;
        }

        MinionConfig minionConfig = _config.MinionConfig;

        if (minionConfig == null) return null;

        CharacterFactory characterFactory =
            _characterFactory.Value;

        if (characterFactory == null) return null;

        Vector3 spawnPosition =
            GetSpawnPosition(summoner, target, spawnIndex);

        CharacterKey minionKey =
            new(summoner.Runtime.TeamType, minionConfig.CharacterType);

        return characterFactory.Spawn(minionKey, spawnPosition);
    }

    private Vector3 GetSpawnPosition(CharacterBrain summoner,
                                     CharacterBrain target,
                                     int spawnIndex) {
        Vector3 summonerPosition =
            summoner.View.transform.position;

        Vector3 direction =
            target.View.transform.position - summonerPosition;

        direction.y = 0f;

        if (direction.sqrMagnitude < MIN_DIRECTION_SQR_MAGNITUDE) {
            direction = summoner.View.transform.forward;
            direction.y = 0f;
        }

        direction.Normalize();

        Vector3 side = new(-direction.z, 0f, direction.x);

        float sideSign =
            spawnIndex % 2 == 0 ? -1f : 1f;

        int sideStep =
            spawnIndex / 2 + 1;

        Vector3 offset =
            direction * _config.MinionForwardOffset +
            side * (_config.MinionSideOffset * sideSign * sideStep);

        return summonerPosition + offset;
    }
}