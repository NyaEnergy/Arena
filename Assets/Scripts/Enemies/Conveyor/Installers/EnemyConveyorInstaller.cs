using UnityEngine;
using Zenject;

public class EnemyConveyorInstaller : MonoInstaller {
    [SerializeField] private EnemyConveyorConfig _config;
    [SerializeField] private EnemyPlatformView _platformPrefab;

    public override void InstallBindings() {
        Container.Bind<EnemyConveyorRuntime>().AsSingle();
        Container.Bind<EnemyConveyorConfig>().FromInstance(_config).AsCached();
        Container.Bind<EnemyPlatformPool>().AsSingle().WithArguments(_platformPrefab);
        Container.BindInterfacesTo<EnemyDirectorService>().AsSingle();
    }
}
