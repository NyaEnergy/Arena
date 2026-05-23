using Zenject;

public class GameplayInstaller : MonoInstaller {
    public override void InstallBindings() {
        Container.Bind<BattlefieldRegistry>().AsSingle();
        Container.Bind<CharacterPool>().AsSingle();
        Container.Bind<CharacterFactory>().AsSingle();
        Container.Bind<DetectionService>().AsSingle();
    }
}
