using UnityEngine;
using Zenject;

public class EnemyDirectorInstaller : MonoInstaller {
    [SerializeField] private EnemyCommanderConfig _config;

    public override void InstallBindings() {
        Container.Bind<EnemyCommanderConfig>()
                 .FromInstance(_config)
                 .AsSingle();

        Container.Bind<EnemyDirectorRuntime>().AsSingle();

        Container.BindInterfacesAndSelfTo<EnemyDirectorService>()
                 .AsSingle();

        Container.BindExecutionOrder<EnemyDirectorService>(-100);
    }
}
