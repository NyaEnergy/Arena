public class MedicAIBehaviorFactory : ICharacterAIBehaviorFactory {
    private readonly MedicTargetSelectionService _targetSelectionService;
    private readonly MedicPositioningService _positioningService;
    private readonly MedicHealingService _healingService;
    private readonly MedicCombatService _combatService;

    public MedicAIBehaviorFactory(MedicTargetSelectionService targetSelectionService,
                                  MedicPositioningService positioningService,
                                  MedicHealingService healingService,
                                  MedicCombatService combatService) {
        _targetSelectionService = targetSelectionService;
        _positioningService = positioningService;
        _healingService = healingService;
        _combatService = combatService;
    }

    public bool CanCreate(CharacterBrain brain) {
        return brain != null &&
               brain.Config.CharacterType == CharacterType.Medic;
    }

    public ICharacterAIBehavior Create(CharacterBrain brain) {
        return new MedicAIBehavior(brain,
                                   _targetSelectionService,
                                   _positioningService,
                                   _healingService,
                                   _combatService);
    }
}
