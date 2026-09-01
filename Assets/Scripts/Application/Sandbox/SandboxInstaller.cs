using ConveyorWars.Presentation.Cameras;
using ConveyorWars.Presentation.Input;
using ConveyorWars.Presentation.Units;
using ConveyorWars.Units;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace ConveyorWars.Application {
    public sealed class SandboxInstaller : MonoInstaller {
        [SerializeField, Required] private SandboxConfig _sandboxConfig;
        [SerializeField, Required] private GameplayControlConfig _gameplayControlConfig;
        [SerializeField, Required] private Camera _mainCamera;
        [SerializeField, Required] private CinemachineView _cameraView;

        public override void InstallBindings() {
            BindConfigs();
            BindSceneComponents();
            BindRuntime();
        }

        private void BindConfigs() {
            Container.BindInstance(_sandboxConfig);
            Container.BindInstance(_gameplayControlConfig);

            Container.Bind<AllyFormationSettings>().AsSingle();
        }

        private void BindSceneComponents() {
            Container.BindInstance(_mainCamera);
            Container.BindInstance(_cameraView);
        }

        private void BindRuntime() {
            Container.Bind<GameplayInputRuntime>().AsSingle();
            Container.Bind<GameplayCommandInput>().AsSingle();
            Container.Bind<GameplaySelectionInput>().AsSingle();

            Container.Bind<SandboxUnitInitializer>().AsSingle();
            Container.Bind<SandboxActiveAllyGroupLifecycle>().AsSingle();

            Container.Bind<UnitCommandProcessor>().AsSingle();
            Container.Bind<UnitRuntimeProcessor>().AsSingle();
            Container.Bind<UnitDecisionProcessor>().AsSingle();
            Container.Bind<UnitStateMachine>().AsSingle();

            Container.Bind<AssistantFormationProcessor>().AsSingle();
            Container.Bind<AssistantCombatCohesionProcessor>().AsSingle();

            Container.Bind<LeaderMarkerPresenter>().AsSingle();

            Container.Bind<SandboxCameraController>().AsSingle();

            Container.Bind<ActiveAllyGroupRuntime>().AsSingle();
            Container.Bind<UnitRegistry>().AsSingle();
            Container.Bind<UnitFactory>().AsSingle();

            Container.BindInterfacesTo<SandboxRuntimeController>().AsSingle();
        }
    }
}