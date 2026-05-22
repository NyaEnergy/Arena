using UnityEngine;
using Zenject;

public class CoreInstaller : MonoInstaller {
    public override void InstallBindings() {
        Application.targetFrameRate = 120;
        BindServices();
    }

    private void BindServices() {
        Container.BindInterfacesAndSelfTo<GameTimeService>().AsSingle();
    }
}
