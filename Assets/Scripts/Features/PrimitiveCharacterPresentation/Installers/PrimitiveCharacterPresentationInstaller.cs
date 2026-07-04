using UnityEngine;
using Zenject;

public class PrimitiveCharacterPresentationInstaller : MonoInstaller {
    [SerializeField] private PrimitiveCharacterPresentationConfig _config;

    public override void InstallBindings() {
        Container.Bind<PrimitiveCharacterPresentationConfig>().FromInstance(_config).AsSingle();
        Container.Bind<PrimitiveCharacterPresentationService>().AsSingle();
    }
}
