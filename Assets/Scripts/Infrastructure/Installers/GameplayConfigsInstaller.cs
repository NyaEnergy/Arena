using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameplayConfigsInstaller : MonoInstaller {
    [SerializeField] private List<CharacterConfig> _characterConfigs;

    public override void InstallBindings() {
        Container.Bind<List<CharacterConfig>>().FromInstance(_characterConfigs).AsCached();
        Container.Bind<CharacterConfigRegistry>().AsSingle();
    }
}
