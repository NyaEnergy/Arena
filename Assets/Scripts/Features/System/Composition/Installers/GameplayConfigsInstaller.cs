using UnityEngine;
using Zenject;

public class GameplayConfigsInstaller : MonoInstaller {
    [SerializeField] private CharacterDeathPresentationConfig _deathPresentationConfig;
    [SerializeField] private HealthBarPaletteConfig _healthBarPalette;

    public override void InstallBindings() {
        Container.Bind<CharacterConfigRegistry>().AsSingle();

        Container.Bind<CharacterDeathPresentationConfig>().FromInstance(_deathPresentationConfig).AsSingle();
        Container.Bind<HealthBarPaletteConfig>().FromInstance(_healthBarPalette).AsSingle();
    }
}