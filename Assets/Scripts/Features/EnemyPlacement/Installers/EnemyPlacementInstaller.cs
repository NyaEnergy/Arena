using UnityEngine;
using Zenject;

public class EnemyPlacementInstaller : MonoInstaller {
    [SerializeField] private QueueDragView _dragView;

    public override void InstallBindings() {
        Container.Bind<QueueDragView>().FromInstance(_dragView).AsSingle();

        Container.Bind<TerritoryDropService>().AsSingle();
        Container.Bind<EnemyGroupFormationService>().AsSingle();
        Container.Bind<EnemyGroupDeploymentService>().AsSingle();
        Container.Bind<EnemyPlacementService>().AsSingle();
        Container.BindInterfacesTo<QueueDragController>().AsSingle();
    }
}