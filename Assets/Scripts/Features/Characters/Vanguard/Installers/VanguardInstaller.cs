using UnityEngine;
using Zenject;

public class VanguardInstaller : MonoInstaller {
    [SerializeField] private VanguardConfig _config;
    [SerializeField] private VanguardView _prefab;

    public override void InstallBindings() {
        Container.Bind<VanguardConfig>().FromInstance(_config).AsSingle();
        Container.Bind<ICharacterConfig>().FromInstance(_config).AsCached();
        Container.Bind<CharacterView>().FromInstance(_prefab).AsCached();
    }
}
