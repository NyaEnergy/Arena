using UnityEngine;
using Zenject;

public sealed class CampaignInstaller : MonoInstaller {
    [SerializeField] private CampaignConfig _config;

    public override void InstallBindings() {
        Container.Bind<CampaignConfig>()
                 .FromInstance(_config)
                 .AsSingle();

        Container.Bind<CampaignProgress>().AsSingle();
        Container.Bind<CommanderQuestProgress>().AsSingle();
        Container.Bind<CommanderProgressionProgress>().AsSingle();
        Container.Bind<CampaignRuntime>().AsSingle();
        Container.Bind<GameplaySceneRequest>().AsSingle();
        Container.Bind<CampaignService>().AsSingle();
        Container.Bind<CampaignSceneFlowService>().AsSingle();
    }
}
