using Zenject;

public class GameplayConfigsInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<CharacterConfigRegistry>().AsSingle();
    }
}
