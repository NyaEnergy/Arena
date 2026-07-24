using UnityEngine;
using Zenject;

public sealed class CommanderProgressionInstaller : MonoInstaller {
    [SerializeField] private CommanderSkillHudView _skillHudView;
    [SerializeField] private CommanderSkillTargetView _skillTargetView;
    [SerializeField] private CommanderProgressionPanelView _progressionPanelView;

    public override void InstallBindings() {

        Container.BindInterfacesAndSelfTo<CommanderSkillService>().AsSingle();
        Container.BindInterfacesTo<CommanderSkillHudController>().AsSingle();
        Container.Bind<CommanderUpgradeEffectService>().AsSingle();
        Container.Bind<CommanderSkillEffectState>().AsSingle();

        Container.BindInterfacesAndSelfTo<CommanderProgressionService>()
                 .AsSingle()
                 .NonLazy();

        Container.Bind<CommanderSkillHudView>()
                 .FromInstance(_skillHudView)
                 .AsSingle();

        Container.Bind<CommanderSkillTargetView>()
                 .FromInstance(_skillTargetView)
                 .AsSingle();


        Container.Bind<CommanderProgressionPanelView>()
                 .FromInstance(_progressionPanelView)
                 .AsSingle();

        Container.BindInterfacesTo<CommanderProgressionPanelController>()
                 .AsSingle();
    }
}
