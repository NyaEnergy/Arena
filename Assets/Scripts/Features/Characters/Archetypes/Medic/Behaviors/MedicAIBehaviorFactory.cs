public class MedicAIBehaviorFactory : ICharacterBehaviorFactory {
    private readonly MedicTargetSelectionService _targetSelectionService;
    private readonly MedicPositioningService _positioningService;
    private readonly MedicHealingService _healingService;
    private readonly MedicCombatService _combatService;
    private readonly MedicRegenerationService _regenerationService;

    public MedicAIBehaviorFactory(MedicTargetSelectionService targetSelectionService,
                                  MedicPositioningService positioningService,
                                  MedicHealingService healingService,
                                  MedicCombatService combatService,
                                  MedicRegenerationService regenerationService) {
        _targetSelectionService = targetSelectionService;
        _positioningService = positioningService;
        _healingService = healingService;
        _combatService = combatService;
        _regenerationService = regenerationService;
    }

    public bool CanCreate(CharacterBrain brain) {
        return brain?.Config is MedicConfig;
    }

    public ICharacterBehavior Create(CharacterBrain brain) {
        return new MedicAIBehavior(brain,
                                  _targetSelectionService,
                                  _positioningService,
                                  _healingService,
                                  _combatService,
                                  _regenerationService);
    }
}