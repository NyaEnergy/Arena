using UnityEngine;

public class HunterAIBehavior : ICharacterBehavior {
    private readonly CharacterBrain _brain;
    private readonly DetectionService _detectionService;
    private readonly HunterCombatModeService _combatModeService;
    private readonly HunterRangedCombatService _rangedCombatService;
    private readonly HunterMeleeCombatService _meleeCombatService;
    private readonly HunterRuntime _runtime;

    public HunterAIBehavior(CharacterBrain brain,
                            DetectionService detectionService,
                            HunterCombatModeService combatModeService,
                            HunterRangedCombatService rangedCombatService,
                            HunterMeleeCombatService meleeCombatService) {
        _brain = brain;
        _detectionService = detectionService;
        _combatModeService = combatModeService;
        _rangedCombatService = rangedCombatService;
        _meleeCombatService = meleeCombatService;

        _runtime = new HunterRuntime();

        Reset();
    }

    public void Reset() {
        _runtime.Reset();
        _brain.TargetComponent.ClearTarget();
    }

    public void Tick() {
        CharacterBrain target = _detectionService.FindClosestTarget(_brain);
        _brain.TargetComponent.SetTarget(target);

        if(target == null) {
            _brain.MovementComponent.Stop();
            return;
        }

        float sqrDistanceToTarget =
            Vector3.SqrMagnitude(_brain.View.transform.position -
                                 target.View.transform.position);

        _combatModeService.UpdateMode(_runtime, sqrDistanceToTarget);

        if (_runtime.CombatMode == HunterCombatMode.Melee) {
            _meleeCombatService.Tick(_brain, target, _runtime,
                                     sqrDistanceToTarget);
            return;
        }

        _rangedCombatService.Tick(_brain, target, _runtime,
                                  sqrDistanceToTarget);
    }
}
