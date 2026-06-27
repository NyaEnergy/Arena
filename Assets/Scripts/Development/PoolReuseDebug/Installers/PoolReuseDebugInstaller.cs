using UnityEngine;
using Zenject;

public class PoolReuseDebugInstaller : MonoInstaller {
    [SerializeField] private PoolReuseDebugView _view;

    [Header("Test Settings")]
    [SerializeField][Min(1)] private int _respawnCount = 5;
    [SerializeField][Min(0f)] private float _killDelay = 0.1f;
    [SerializeField][Min(0f)] private float _respawnDelay = 0.25f;
    [SerializeField][Min(0.1f)] private float _despawnTimeout = 2f;

    public override void InstallBindings() {
        PoolReuseDebugSettings settings = new(_respawnCount,
                                              _killDelay,
                                              _respawnDelay,
                                              _despawnTimeout);

        Container.Bind<PoolReuseDebugSettings>().FromInstance(settings).AsSingle();
        Container.Bind<PoolReuseDebugView>().FromInstance(_view).AsSingle();

        Container.BindInterfacesTo<PoolReuseDebugService>().AsSingle();

        Container.Bind<PoolReuseDebugCharacterService>().AsSingle();
        Container.Bind<PoolReuseDebugValidator>().AsSingle();
        Container.Bind<PoolReuseDebugTracker>().AsSingle();
        Container.Bind<PoolReuseDebugPresenter>().AsSingle();
    }
}
