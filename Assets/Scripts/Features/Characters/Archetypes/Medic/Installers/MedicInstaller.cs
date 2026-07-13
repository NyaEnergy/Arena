using UnityEngine;
using Zenject;

public class MedicInstaller : MonoInstaller {
    [SerializeField] private MedicConfig _config;

    public override void InstallBindings() {
        Container.Bind<ICharacterBehaviorFactory>().To<MedicAIBehaviorFactory>().AsSingle();

        Container.Bind<ICharacterConfig>().FromInstance(_config).AsCached();
        Container.Bind<MedicConfig>().FromInstance(_config).AsSingle();

        Container.Bind<MedicEmergencySwitchService>().AsSingle();
        Container.Bind<MedicTargetSelectionService>().AsSingle();
        Container.Bind<MedicPositioningService>().AsSingle();
        Container.Bind<MedicAllyQueryService>().AsSingle();
        Container.Bind<MedicHealingService>().AsSingle();
        Container.Bind<MedicHealthService>().AsSingle();
        Container.Bind<MedicCombatService>().AsSingle();
    }
}
