using System;
using Zenject;

public sealed class AllyCommanderInstaller : MonoInstaller {
    [Inject] private GameplaySceneSettings _sceneSettings;

    public override void InstallBindings() {
        AllyCommanderConfig commander = _sceneSettings?.AlliedCommander;

        if (commander == null || commander.TeamType != TeamType.Ally) {
            throw new InvalidOperationException(
                "Allied commander must be selected before " +
                "installing the allied side.");
        }

        Container.Bind<AllyCommanderConfig>() .FromInstance(commander) .AsSingle();
    }
}
