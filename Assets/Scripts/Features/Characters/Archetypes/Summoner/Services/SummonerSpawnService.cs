using UnityEngine;
using Zenject;

public class SummonerSpawnService {
    private readonly LazyInject<SummonedCharacterFactory> _summonedCharacterFactory;
    private readonly SummonerSpawnPositionService _positionService;

    public SummonerSpawnService(LazyInject<SummonedCharacterFactory> summonedCharacterFactory,
                                SummonerSpawnPositionService positionService) {
        _summonedCharacterFactory = summonedCharacterFactory;
        _positionService = positionService;
    }

    public CharacterView Spawn(CharacterBrain summoner,
                               CharacterBrain target,
                               SummonerConfig config,
                               int spawnIndex) {

        SummonedCharacterConfig summonedConfig =
            config?.SummonedCharacterConfig;

        if (summonedConfig == null) {
            return null;
        }

        if (!_positionService.TryGet(
                summoner,
                target,
                config,
                spawnIndex,
                out Vector3 destination))
                    return null;

        SummonedCharacterFactory factory =
            _summonedCharacterFactory.Value;

        if (factory == null) return null;

        bool usesArc = summonedConfig.EntryPresentation is CharacterArcPresenceConfig;

        Vector3 startPosition = usesArc ?
            GetSummonOrigin(summoner) : destination;

        CharacterView character =
            factory.Prepare(summoner.Runtime.TeamType,
                            summonedConfig,
                            startPosition);

        if (character == null) return null;

        Quaternion endRotation =
            GetTargetRotation(target,
                              destination);

        if (usesArc) {
            BeginArc(character,
                     summoner,
                     startPosition,
                     destination,
                     endRotation);

            return character;
        }

        character.transform.SetPositionAndRotation(destination,
                                                   endRotation);

        character.EnterBattlefield();
        return character;
    }

    private void BeginArc(CharacterView character,
                          CharacterBrain summoner,
                          Vector3 startPosition,
                          Vector3 destination,
                          Quaternion endRotation) {

        Quaternion startRotation =
            summoner.View.transform.rotation;

        CharacterPresenceTransitionRequest request =
            new(CharacterPresenceTransitionDirection.Enter,
                startPosition,
                destination,
                startRotation,
                endRotation);

        character.EnterBattlefield(request);
    }

    private Vector3 GetSummonOrigin(CharacterBrain summoner) {
        if (summoner.View is SummonerView view) {
            return view.SummonOriginPosition;
        }

        return summoner.View.AimPosition;
    }

    private Quaternion GetTargetRotation(CharacterBrain target,
                                         Vector3 position) {
        Vector3 direction = target.View.transform.position -
                            position;

        direction.y = 0f;

        if (direction.sqrMagnitude <= 0.001f) {
            return Quaternion.identity;
        }

        return Quaternion.LookRotation(direction.normalized,
                                       Vector3.up);
    }
}