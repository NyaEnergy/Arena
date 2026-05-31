using UnityEngine;
using Zenject;

public class EnemyConveyorInstaller : MonoInstaller {
    [SerializeField] private EnemyConveyorConfig _config;

    public override void InstallBindings() {
        Container.Bind<EnemyConveyorRuntime>().AsSingle();
        
        Container.Bind<EnemyConveyorConfig>().FromInstance(_config).AsCached();

        Container.BindInterfacesTo<EnemyDirectorService>().AsSingle();
        Container.BindInterfacesTo<EnemyConveyorService>().AsSingle();
    }
}
