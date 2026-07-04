using UnityEngine;

public class HunterAIBehaviorFactory : ICharacterAIBehaviorFactory {
    private readonly DetectionService _detectionService;
    private readonly HunterCombatModeService _combatModeService;
    private readonly HunterRangedCombatService _rangedCombatService;
    private readonly HunterMeleeCombatService _meleeCombatService;

    public HunterAIBehaviorFactory(DetectionService detectionService,
                                   HunterCombatModeService combatModeService,
                                   HunterRangedCombatService rangedCombatService,
                                   HunterMeleeCombatService meleeCombatService) {
        _detectionService = detectionService;
        _combatModeService = combatModeService;
        _rangedCombatService = rangedCombatService;
        _meleeCombatService = meleeCombatService;
    }

    public bool CanCreate(CharacterBrain brain) {
        return brain != null &&
               brain.Config.CharacterType == CharacterType.Hunter;
    }

    public ICharacterAIBehavior Create(CharacterBrain brain) {
        return new HunterAIBehavior(brain,
                                    _detectionService,
                                    _combatModeService,
                                    _rangedCombatService,
                                    _meleeCombatService);
    }
}
