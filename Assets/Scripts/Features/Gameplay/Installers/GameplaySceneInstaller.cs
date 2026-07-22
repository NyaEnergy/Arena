using System;
using UnityEngine;
using Zenject;

public sealed class GameplaySceneInstaller : MonoInstaller {
    [Header("Direct Scene Play")]
    [SerializeField] private AllyCommanderConfig _alliedCommander;
    [SerializeField] private EnemyCommanderConfig _enemyCommander;
    [SerializeField] private StoryTerritoryConfig _territory;

    [InjectOptional]
    private GameplaySceneRequest _sceneRequest;

    public override void InstallBindings() {
        GameplaySceneSettings settings = ResolveSettings();

        if (!settings.IsValid) {
            throw new InvalidOperationException(
                "Gameplay scene requires valid allied commander, " +
                "enemy commander and story territory settings.");
        }

        Container.Bind<GameplaySceneSettings>()
                 .FromInstance(settings)
                 .AsSingle();

        Container.Bind<StoryTerritoryConfig>()
                 .FromInstance(settings.Territory)
                 .AsSingle();
    }

    private GameplaySceneSettings ResolveSettings() {
        if (_sceneRequest != null &&
            _sceneRequest.TryGet(out GameplaySceneSettings settings)) {
            return settings;
        }

        return new GameplaySceneSettings(
            _alliedCommander,
            _enemyCommander,
            _territory);
    }
}
