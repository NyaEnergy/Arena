using Zenject;
using UnityEngine;

public class GameplayInstaller : MonoInstaller {
    [SerializeField]
    private CharacterSpawnerInstaller _characterSpawnerInstaller;

    public override void InstallBindings() {
        Container.Bind<CharacterSpawnerInstaller>().FromInstance(_characterSpawnerInstaller).AsCached();
        Container.Bind<BattlefieldRegistry>().AsSingle();
        Container.Bind<CharacterPool>().AsSingle();
        Container.Bind<CharacterFactory>().AsSingle();

        Container.Bind<DetectionService>().AsSingle();
        Container.Bind<UtilityAIService>().AsSingle();
        Container.BindInterfacesTo<CharacterBootstrap>().AsSingle();

        Container.BindInterfacesAndSelfTo<PressureService>().AsSingle();
    }
}
