using UnityEngine;

public class SummonerSummonService {
    private readonly SummonerSpawnService _spawnService;

    public SummonerSummonService(SummonerSpawnService spawnService) {
        _spawnService = spawnService;
    }

    public void TrySummon(CharacterBrain summoner,
                          CharacterBrain target,
                          SummonerConfig config,
                          SummonerRuntime runtime) {
        
        if (!runtime.IsReady(Time.time)) return;
        if (!runtime.HasFreeSlot(config.MaxSummons)) return;

        CharacterView character =
            _spawnService.Spawn(summoner,
                                target,
                                config,
                                runtime.SpawnIndex);

        if (character == null) return;

        runtime.Register(character,
                         Time.time,
                         config.SummonCooldown);

        if (summoner.View is SummonerView view) {
            view.PlaySummon();
        }
    }
}