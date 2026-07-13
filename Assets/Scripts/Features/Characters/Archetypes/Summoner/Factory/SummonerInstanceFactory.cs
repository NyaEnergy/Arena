using System;
using Zenject;

public class SummonerInstanceFactory {
    private readonly DiContainer _container;
    private readonly CharacterLifecycleFactory _lifecycleFactory;
    private readonly SummonerDeathEventService _deathEventService;

    public SummonerInstanceFactory(
                DiContainer container,
                CharacterLifecycleFactory lifecycleFactory,
                SummonerDeathEventService deathEventService) {

        _container = container;
        _lifecycleFactory = lifecycleFactory;
        _deathEventService = deathEventService;
    }

    public SummonerView Create(SummonerPoolKey key,
                               Action<CharacterView> returnToPool) {

        SummonerView prefab = key.Config?.Prefab as SummonerView;

        if (prefab == null ||
            returnToPool == null) {
            return null;
        }

        SummonerView view =
            _container.InstantiatePrefabForComponent<SummonerView>(prefab);

        CharacterLifecycleController controller =
            _lifecycleFactory.Create(
                view,
                key.TeamType,
                key.Config,
                character =>
                    NotifyDeath(key, character),
                returnToPool
            );

        if (controller == null ||
            !view.Initialize(controller)) {

            UnityEngine.Object.Destroy(view.gameObject);
            return null;
        }

        return view;
    }

    private void NotifyDeath(SummonerPoolKey key,
                             CharacterView view) {
        
        _deathEventService.NotifyDeath(
            new SummonerDeathInfo(
                key, view.transform.position));
    }
}