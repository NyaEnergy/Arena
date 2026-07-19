using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DemoBattleInstaller : MonoInstaller {
    [SerializeField] private List<DemoBattleCallEntry> _callEntries = new();

    public override void InstallBindings() {
        BindCall();
        BindCamera();
    }

    private void BindCall() {
        Container.Bind<IReadOnlyList<DemoBattleCallEntry>>().FromInstance(_callEntries).AsSingle();
        Container.BindInterfacesTo<DemoBattleCallService>().AsSingle();
        Container.Bind<DemoBattleCallSpawnService>().AsSingle();
    }

    private void BindCamera() {
        Container.Bind<DemoBattleCombatCenterService>().AsSingle();
        Container.Bind<DemoBattleCameraFollowRuntime>().AsSingle();
        Container.Bind<DemoBattleCameraSmoothingService>().AsSingle();
        Container.Bind<DemoBattleCameraMovementService>().AsSingle();
        Container.Bind<DemoBattleCameraZoomService>().AsSingle();
        Container.BindInterfacesTo <DemoBattleCameraFollowService>().AsSingle();
    }
}