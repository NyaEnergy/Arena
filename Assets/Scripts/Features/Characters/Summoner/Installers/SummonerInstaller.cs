using UnityEngine;
using Zenject;

public class SummonerInstaller : MonoInstaller {
    [SerializeField] private SummonerConfig _config;

    public override void InstallBindings() {
        Container.Bind<ICharacterAIBehaviorFactory>().To<SummonerAIBehaviorFactory>().AsSingle();

        Container.Bind<ICharacterConfig>().FromInstance(_config).AsCached();
        Container.Bind<SummonerConfig>().FromInstance(_config).AsSingle();

        Container.Bind<SummonerMinionSpawnService>().AsSingle();
    }
}