using UnityEngine;
using Zenject;

public class EnemyDirectorInstaller : MonoInstaller {
    [SerializeField] private EnemyDirectorConfig _config;

    public override void InstallBindings() {
        Container.Bind<EnemyDirectorConfig>()
                 .FromInstance(_config)
                 .AsSingle();

        Container.Bind<EnemyDirectorRuntime>().AsSingle();

        Container
            .BindInterfacesAndSelfTo<EnemyDirectorService>()
            .AsSingle();

        Container.BindExecutionOrder<EnemyDirectorService>(-100);
    }
}
