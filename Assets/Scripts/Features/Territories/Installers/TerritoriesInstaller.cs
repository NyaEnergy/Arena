using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class TerritoriesInstaller : MonoInstaller {
    [SerializeField]
    private List<TerritoryView> _territories =
        new();

    public override void InstallBindings() {
        Container.Bind<IReadOnlyList<TerritoryView>>().FromInstance(_territories).AsSingle();

        Container.BindInterfacesTo<TerritoryStartService>().AsSingle();

        Container.Bind<TerritoryPointService>().AsSingle();
        Container.Bind<TerritoryHeroService>().AsSingle();
        Container.Bind<TerritorySpawnGate>().AsSingle();
        Container.Bind<TerritoryRegistry>().AsSingle();
    }
}