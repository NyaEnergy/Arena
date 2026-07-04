using UnityEngine;

public class HunterMeleeCombatService {
    private const float ATTACK_DISTANCE_BUFFER = 0.2f;

    private readonly HunterConfig _config;
    private readonly HunterAttackService _attackService;

    public HunterMeleeCombatService(HunterConfig config,
                                    HunterAttackService attackService) {
        _config = config;
        _attackService = attackService;
    }

    public void Tick(CharacterBrain hunter,
                     CharacterBrain target,
                     HunterRuntime runtime,
                     float sqrDistanceToTarget) {
        float maximumAttackDistance = _config.MeleeAttackDistanceRange.Max;
        float sqrMaximumAttackDistance = maximumAttackDistance * maximumAttackDistance;

        if(sqrDistanceToTarget > sqrMaximumAttackDistance) {
            hunter.MovementComponent.MoveToDistance(target.View.transform.position,
                                                    Mathf.Max(0f, maximumAttackDistance - ATTACK_DISTANCE_BUFFER), 1);
            return;
        }

        hunter.MovementComponent.Stop();

        _attackService.TryMeleeAttack(hunter, target, runtime);
    }
}
