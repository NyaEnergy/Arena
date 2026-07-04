using Zenject;

public class CharacterSharedInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<CharacterLineOfSightService>().AsSingle();
    }
}
