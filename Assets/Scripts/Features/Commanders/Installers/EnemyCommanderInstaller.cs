using System;
using Zenject;

public sealed class EnemyCommanderInstaller : MonoInstaller {
    [Inject] private GameplaySceneSettings _sceneSettings;

    public override void InstallBindings() {
        EnemyCommanderConfig commander = _sceneSettings?.EnemyCommander;

        if (commander == null ||
            commander.TeamType != TeamType.Enemy) {
            throw new InvalidOperationException(
                "Enemy commander must be selected before " +
                "installing the enemy side.");
        }

        Container.Bind<EnemyCommanderConfig>()
                 .FromInstance(commander)
                 .AsSingle();

        InstallEnemyDirector();
    }

    private void InstallEnemyDirector() {
        Container.Bind<EnemyDirectorRuntime>().AsSingle();

        Container.BindInterfacesAndSelfTo<EnemyDirectorService>().AsSingle();
        Container.BindExecutionOrder<EnemyDirectorService>(-100);
    }
}
