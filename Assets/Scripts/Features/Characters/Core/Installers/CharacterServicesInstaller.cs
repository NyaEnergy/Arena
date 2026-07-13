using Zenject;

public class CharacterServicesInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<CharacterLineOfSightService>().AsSingle();
    }
}