using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller {
    [SerializeField] private Camera _camera;

    public override void InstallBindings() {
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();

        Container.Bind<CharacterDeathEventService>().AsSingle();

        Container.Bind<CharacterControllerFactory>().AsSingle();
        Container.Bind<BattlefieldRegistry>().AsSingle();
        Container.Bind<CharacterFactory>().AsSingle();
        Container.Bind<DetectionService>().AsSingle();
        Container.Bind<UtilityAIService>().AsSingle();
        Container.Bind<CharacterPool>().AsSingle();
    }
}