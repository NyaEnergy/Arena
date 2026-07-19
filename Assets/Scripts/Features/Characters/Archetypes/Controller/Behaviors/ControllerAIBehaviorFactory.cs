public class ControllerAIBehaviorFactory : ICharacterBehaviorFactory {

    private readonly DetectionService _detectionService;
    private readonly ControllerPositioningService _positioningService;
    private readonly ControllerFieldService _fieldService;

    public ControllerAIBehaviorFactory(
                DetectionService detectionService,
                ControllerPositioningService positioningService,
                ControllerFieldService fieldService) {

        _detectionService = detectionService;
        _positioningService = positioningService;
        _fieldService = fieldService;
    }

    public bool CanCreate(CharacterBrain brain) {

        return brain?.Config is ControllerConfig;
    }

    public ICharacterBehavior Create(CharacterBrain brain) {

        return new ControllerAIBehavior(brain,
            brain.Config as ControllerConfig,
            _detectionService,
            _positioningService,
            _fieldService);
    }
}