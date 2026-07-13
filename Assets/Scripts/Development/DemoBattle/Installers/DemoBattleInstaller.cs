using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DemoBattleInstaller : MonoInstaller {
    [Header("Call Points")]
    [SerializeField] private Transform _allyCallPoint;

    [SerializeField] private Transform _enemyCallPoint;

    [Header("Call Buttons")]
    [SerializeField] private List<DemoBattleCallEntry> _callEntries = new();

    public override void InstallBindings() {
        BindCall();
        BindCamera();
    }

    private void BindCall() {
        Container.Bind<IReadOnlyList<DemoBattleCallEntry>>().FromInstance(_callEntries).AsSingle();

        Container.Bind<DemoBattleCallPositionService>()
            .FromInstance(new DemoBattleCallPositionService(_allyCallPoint,
                                                            _enemyCallPoint))
            .AsSingle();

        Container.Bind<DemoBattleCallSpawnService>().AsSingle();
        Container.BindInterfacesTo<DemoBattleCallService>().AsSingle();
    }

    private void BindCamera() {
        Container.Bind<DemoBattleCombatCenterService>().AsSingle();
        Container.Bind<DemoBattleCameraFollowRuntime>().AsSingle();
        Container.Bind<DemoBattleCameraSmoothingService>().AsSingle();
        Container.Bind<DemoBattleCameraMovementService>().AsSingle();
        Container.Bind<DemoBattleCameraRotationService>().AsSingle();
        Container.Bind<DemoBattleCameraZoomService>().AsSingle();
        Container.BindInterfacesTo <DemoBattleCameraFollowService>() .AsSingle();
    }
}