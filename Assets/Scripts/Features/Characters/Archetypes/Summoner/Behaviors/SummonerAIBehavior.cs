public class SummonerAIBehavior : ICharacterBehavior {
    private readonly CharacterBrain _brain;
    private readonly SummonerConfig _config;
    private readonly DetectionService _detectionService;
    private readonly SummonerMovementService _movementService;
    private readonly SummonerSummonService _summonService;
    private readonly SummonerRuntime _runtime = new();

    public SummonerAIBehavior(CharacterBrain brain,
                              SummonerConfig config,
                              DetectionService detectionService,
                              SummonerMovementService movementService,
                              SummonerSummonService summonService) {
        _brain = brain;
        _config = config;
        _detectionService = detectionService;
        _movementService = movementService;
        _summonService = summonService;

        Reset();
    }

    public void Reset() {
        _runtime.Reset();
        _brain.TargetComponent.ClearTarget();
        _brain.MovementComponent.Stop();
    }

    public void Tick() {
        _runtime.Clean();

        CharacterBrain target =
            _detectionService.FindClosestTarget(_brain);

        _brain.TargetComponent.SetTarget(target);

        if (target == null) {
            _brain.MovementComponent.Stop();
            return;
        }

        if (!_movementService.Tick(
                _brain, target, _config)) {
            return;
        }

        _summonService.TrySummon(
            _brain,
            target,
            _config,
            _runtime);
    }
}