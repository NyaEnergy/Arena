public class SummonerAIBehaviorFactory : ICharacterBehaviorFactory {
    private readonly DetectionService _detectionService;
    private readonly SummonerMovementService _movementService;
    private readonly SummonerSummonService _summonService;

    public SummonerAIBehaviorFactory(DetectionService detectionService,
                                     SummonerMovementService movementService,
                                     SummonerSummonService summonService) {
        _detectionService = detectionService;
        _movementService = movementService;
        _summonService = summonService;
    }

    public bool CanCreate(CharacterBrain brain) {
        return brain?.Config is SummonerConfig;
    }

    public ICharacterBehavior Create(CharacterBrain brain) {
        return new SummonerAIBehavior(
            brain, brain.Config as SummonerConfig,
            _detectionService,
            _movementService,
            _summonService);
    }
}