using UnityEngine;
using Zenject;

public class GameplayConfigsInstaller : MonoInstaller {
    [SerializeField] private TerritoryConfig _territoryConfigs;

    public override void InstallBindings() {
        Container.Bind<TerritoryConfig>().FromInstance(_territoryConfigs).AsSingle();
    }
}
