using System.Collections.Generic;
using UnityEngine;

public class TerritorySpawnGate {
    private readonly TerritoryRegistry _registry;
    private readonly TerritoryHeroService _heroService;
    private readonly TerritoryPointService _pointService;

    public TerritorySpawnGate(TerritoryRegistry registry,
                              TerritoryHeroService heroService,
                              TerritoryPointService pointService) {
        _registry = registry;
        _heroService = heroService;
        _pointService = pointService;
    }

    public bool TryGetEnemyPosition(out Vector3 position) {
        position = default;

        IReadOnlyList<TerritoryRuntime> territories =
            _registry.Territories;

        for (int i = 0; i < territories.Count; i++) {
            if (TryGetEnemyPosition(territories[i], out position)) {
                return true;
            }
        }

        return false;
    }

    public bool TryGetEnemyPosition(TerritoryRuntime territory,
                                out Vector3 position) {

        position = default;

        if (!CanSpawn(territory)) return false;

        return _pointService.TryGet(
            territory, out position);
    }

    public bool CanSpawn(TerritoryRuntime territory) {
        return territory != null &&
               territory.IsSpawnEnabled &&
              _heroService.HasLivingAlly(territory);
    }
}