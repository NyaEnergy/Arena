using UnityEngine;

public class HunterRangedCombatService {
    private const float ATTACK_DISTANCE_BUFFER = 0.2f;

    private readonly HunterConfig _config;
    private readonly HunterVanguardQueryService _vanguardQueryService;
    private readonly HunterPositioningService _positioningService;
    private readonly HunterAttackService _attackService;

    public HunterRangedCombatService(HunterConfig config,
                                     HunterVanguardQueryService vanguardQueryService,
                                     HunterPositioningService positioningService,
                                     HunterAttackService attackService) {
        _config = config;
        _vanguardQueryService = vanguardQueryService;
        _positioningService = positioningService;
        _attackService = attackService;
    }

    public void Tick(CharacterBrain hunter,
                     CharacterBrain target,
                     HunterRuntime runtime,
                     float sqrDistanceToTarget) {

        CharacterBrain vanguard = _vanguardQueryService.FindClosest(hunter);

        if (vanguard != null) {
            FollowVanguard(hunter, target, vanguard);
            TryAttack(hunter, target, runtime);
            return;
        }

        TickWithoutVanguard(hunter, target, runtime,
                            sqrDistanceToTarget);
    }

    private void FollowVanguard(CharacterBrain hunter,
                                CharacterBrain target,
                                CharacterBrain vanguard) {
        Vector3 position =
            _positioningService.GetPositionBehindVanguard(
                    hunter, vanguard, target);

        hunter.MovementComponent.MoveToPosition(position);
    }

    private void TickWithoutVanguard(CharacterBrain hunter,
                                     CharacterBrain target,
                                     HunterRuntime runtime,
                                     float sqrDistanceToTarget) {
        float maximumAttackDistance =
            _config.RangedAttackDistanceRange.Max;

        float sqrMaximumAttackDistance = maximumAttackDistance *
                                         maximumAttackDistance;

        if (sqrDistanceToTarget > sqrMaximumAttackDistance) {

            hunter.MovementComponent.MoveToDistance(
                    target.View.transform.position,
                    Mathf.Max(0f, maximumAttackDistance -
                                  ATTACK_DISTANCE_BUFFER),
                    1f);

            return;
        }

        Vector3 kitePosition = _positioningService.GetKitePosition(hunter, target);

        hunter.MovementComponent.MoveToPosition(kitePosition,
                                                _config.KiteSpeedMultiplier);

        TryAttack(
            hunter,
            target,
            runtime);
    }

    private void TryAttack(CharacterBrain hunter,
                           CharacterBrain target,
                           HunterRuntime runtime) {
        _attackService.TryRangedAttack(hunter, target, runtime);
    }
}
