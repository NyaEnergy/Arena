using UnityEngine;
using Zenject;

public class EnemyConveyorInstaller : MonoInstaller {
    [SerializeField] private EnemyConveyorConfig _config;
    [SerializeField] private EnemyPlatformView _platformPrefab;
    [SerializeField] private EnemyConveyorRoot _conveyorRoot;

    public override void InstallBindings() {
        Container.Bind<EnemyConveyorRuntime>().AsSingle();
        Container.Bind<EnemyConveyorConfig>().FromInstance(_config).AsCached();
        Container.Bind<EnemyPlatformView>().FromInstance(_platformPrefab).AsCached();
        Container.Bind<EnemyConveyorRoot>().FromInstance(_conveyorRoot).AsCached();
        Container.Bind<EnemyPlatformPool>().AsSingle();
        Container.BindInterfacesTo<EnemyDirectorService>().AsSingle();
        Container.Bind<EnemyConveyorLayoutService>().AsSingle();
    }
}
