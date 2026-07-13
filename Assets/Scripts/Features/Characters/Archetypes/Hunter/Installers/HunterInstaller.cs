using UnityEngine;
using Zenject;

public class HunterInstaller : MonoInstaller {
    [SerializeField] private HunterConfig _config;

    public override void InstallBindings() {
        Container.Bind<ICharacterBehaviorFactory>().To<HunterAIBehaviorFactory>().AsSingle();

        Container.Bind<ICharacterConfig>().FromInstance(_config).AsCached();
        Container.Bind<HunterConfig>().FromInstance(_config).AsSingle();

        Container.Bind<HunterVanguardQueryService>().AsSingle();
        Container.Bind<HunterRangedCombatService>().AsSingle();
        Container.Bind<HunterPositioningService>().AsSingle();
        Container.Bind<HunterMeleeCombatService>().AsSingle();
        Container.Bind<HunterCombatModeService>().AsSingle();
        Container.Bind<HunterAttackService>().AsSingle();
    }
}
