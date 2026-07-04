using UnityEngine;
using Zenject;

public class AllySquadInstaller : MonoInstaller {
    [SerializeField] private AllySquadView _view;

    public override void InstallBindings() {
        Container.Bind<AllySquadView>().FromInstance(_view).AsSingle();
        Container.BindInterfacesTo<AllySquadSpawnService>().AsSingle();
    }
}
