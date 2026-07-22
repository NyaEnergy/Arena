using Zenject;

public sealed class CommanderProgressionInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.BindInterfacesAndSelfTo<CommanderProgressionService>()
                 .AsSingle()
                 .NonLazy();

        Container.Bind<CommanderUpgradeEffectService>().AsSingle();
        Container.Bind<CommanderSkillEffectState>().AsSingle();

        Container.BindInterfacesAndSelfTo<CommanderSkillService>()
                 .AsSingle();
    }
}
