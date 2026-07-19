using UnityEngine;
using Zenject;

public class EnemyQueueInstaller : MonoInstaller {
    [SerializeField] private EnemyQueueView _view;
    [SerializeField] private EnemyConveyorConfig _conveyorConfig;

    public override void InstallBindings() {
        Container.Bind<EnemyQueueView>().FromInstance(_view).AsSingle();
        Container.Bind<EnemyQueueRuntime>().AsSingle();
        Container.Bind<EnemyQueueService>().AsSingle();
        Container.Bind<EnemyQueueReleaseService>().AsSingle();

        Container.Bind<EnemyConveyorConfig>()
                 .FromInstance(_conveyorConfig)
                 .AsSingle();

        Container.Bind<EnemyConveyorRuntime>().AsSingle();
        Container.Bind<EnemyConveyorSource>().AsSingle();
        Container.BindInterfacesAndSelfTo<EnemyConveyorService>().AsSingle();

        Container.BindInterfacesTo<EnemyQueueController>().AsSingle();
    }
}