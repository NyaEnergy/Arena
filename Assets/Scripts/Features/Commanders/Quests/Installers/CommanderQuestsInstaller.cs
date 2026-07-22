using Zenject;

public sealed class CommanderQuestsInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<CommanderQuestService>().AsSingle().NonLazy();
    }
}
