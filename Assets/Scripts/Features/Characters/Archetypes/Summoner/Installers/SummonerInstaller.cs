using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SummonerInstaller : MonoInstaller {
    [SerializeField] private List<SummonerConfig> _summonerConfigs = new();

    public override void InstallBindings() {
        BindConfigs();
        BindBehaviors();
        BindSummoning();
        BindCreation();
    }

    private void BindConfigs() {
        Container.Bind<IReadOnlyList<SummonerConfig>>().FromInstance(_summonerConfigs) .AsSingle();
        Container.Bind<SummonerConfigCollection>().AsSingle();
    }

    private void BindBehaviors() {
        Container.Bind<ICharacterBehaviorFactory>().To<SummonerAIBehaviorFactory>() .AsSingle();
        Container.Bind<ICharacterBehaviorFactory>().To<TurretAIBehaviorFactory>() .AsSingle();
        Container.Bind<TurretTargetQueryService>().AsSingle();
        Container.Bind<TurretShotService>().AsSingle();
    }

    private void BindSummoning() {
        Container.Bind<SummonerMovementService>().AsSingle();
        Container.Bind<SummonerSpawnPositionService>().AsSingle();
        Container.Bind<SummonerSpawnService>().AsSingle();
        Container.Bind<SummonerSummonService>().AsSingle();
        Container.Bind<SummonerDeathEventService>().AsSingle();
    }

    private void BindCreation() {
        Container.Bind<SummonerInstanceFactory>().AsSingle();
        Container.Bind<SummonerPool>().AsSingle();
        Container.Bind<SummonerFactory>().AsSingle();
        Container.Bind<SummonedCharacterPool>().AsSingle();
        Container.Bind<SummonedCharacterFactory>().AsSingle();
    }
}