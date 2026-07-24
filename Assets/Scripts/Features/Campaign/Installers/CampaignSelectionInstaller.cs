using UnityEngine;
using Zenject;

public sealed class CampaignSelectionInstaller : MonoInstaller {
    [SerializeField] private CampaignSelectionView _view;

    public override void InstallBindings() {
        Container.Bind<CampaignSelectionView>()
                 .FromInstance(_view)
                 .AsSingle();

        Container.BindInterfacesTo<CampaignSelectionController>()
                 .AsSingle();
    }
}
