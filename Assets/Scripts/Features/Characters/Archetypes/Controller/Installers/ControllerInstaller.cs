using UnityEngine;
using Zenject;

public class ControllerInstaller : MonoInstaller {

    [SerializeField] private ControllerConfig _config;

    public override void InstallBindings() {
        Container.Bind<ICharacterBehaviorFactory>().To<ControllerAIBehaviorFactory>().AsSingle();

        Container.Bind<ICharacterConfig>().FromInstance(_config).AsCached();
        Container.Bind<ControllerConfig>().FromInstance(_config).AsSingle();

        Container.BindInterfacesAndSelfTo<ControllerSlowService>().AsSingle();
        Container.BindInterfacesAndSelfTo<ControllerFieldService>().AsSingle();

        Container.Bind<ControllerFieldPool>().AsSingle();
        Container.Bind<ControllerPositioningService>().AsSingle();
    }
}