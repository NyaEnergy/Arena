public class TurretAIBehaviorFactory : ICharacterBehaviorFactory {
    private readonly TurretTargetQueryService _targetQueryService;
    private readonly TurretShotService _shotService;

    public TurretAIBehaviorFactory(TurretTargetQueryService targetQueryService,
                                   TurretShotService shotService) {

        _targetQueryService = targetQueryService;
        _shotService = shotService;
    }

    public bool CanCreate(CharacterBrain brain) {
        return brain?.Config is TurretConfig;
    }

    public ICharacterBehavior Create(CharacterBrain brain) {
        return new TurretAIBehavior(brain,
            brain.Config as TurretConfig,
            _targetQueryService,
            _shotService
        );
    }
}