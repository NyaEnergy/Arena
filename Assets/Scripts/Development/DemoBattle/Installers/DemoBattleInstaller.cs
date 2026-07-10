using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DemoBattleInstaller : MonoInstaller {
    [SerializeField] private List<DemoBattleSpawnEntry> _spawnEntries = new();

    public IReadOnlyList<DemoBattleSpawnEntry> SpawnEntries => _spawnEntries;

    public override void InstallBindings() {
        Container.Bind<IReadOnlyList<DemoBattleSpawnEntry>>().FromInstance(_spawnEntries).AsSingle();

        Container.Bind<DemoBattleCombatCenterService>().AsSingle();

        Container.BindInterfacesTo<DemoBattleSpawnService>().AsSingle();
        Container.BindInterfacesTo<DemoBattleAutoRespawnService>().AsSingle();
        Container.BindInterfacesTo<DemoBattleCameraFollowService>().AsSingle();
    }
}