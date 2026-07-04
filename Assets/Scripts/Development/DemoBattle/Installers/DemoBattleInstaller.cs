using UnityEngine;
using Zenject;

public class DemoBattleInstaller : MonoInstaller {
    [SerializeField] private DemoBattleView _view;

    public override void InstallBindings() {
        Container.Bind<DemoBattleView>().FromInstance(_view).AsSingle();
        Container.BindInterfacesTo<DemoBattleSpawnService>().AsSingle();
    }
}