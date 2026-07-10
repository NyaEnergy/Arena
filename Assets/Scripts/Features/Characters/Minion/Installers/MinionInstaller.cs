using UnityEngine;
using Zenject;

public class MinionInstaller : MonoInstaller {
    [SerializeField] private MinionConfig _config;

    public override void InstallBindings() {
        Container.Bind<ICharacterConfig>().FromInstance(_config).AsCached();
        Container.Bind<MinionConfig>().FromInstance(_config).AsSingle();
    }
}