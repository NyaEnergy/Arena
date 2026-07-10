public class SummonerAIBehaviorFactory : ICharacterAIBehaviorFactory {
    private readonly SummonerConfig _config;
    private readonly DetectionService _detectionService;
    private readonly SummonerMinionSpawnService _spawnService;

    public SummonerAIBehaviorFactory(SummonerConfig config,
                                     DetectionService detectionService,
                                     SummonerMinionSpawnService spawnService) {
        _config = config;
        _detectionService = detectionService;
        _spawnService = spawnService;
    }

    public bool CanCreate(CharacterBrain brain) {
        return brain != null &&
               brain.Config.CharacterType == CharacterType.Summoner;
    }

    public ICharacterAIBehavior Create(CharacterBrain brain) {
        return new SummonerAIBehavior(brain,
                                      _config,
                                      _detectionService,
                                      _spawnService);
    }
}