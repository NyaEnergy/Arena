using UnityEngine;
using Zenject;

public class GameplayInstaller : MonoInstaller {
    [SerializeField]
    private Camera _camera;

    public override void InstallBindings() {
        Container.Bind<Camera>().FromInstance(_camera).AsSingle();
        BindCharacterCore();
        BindPresence();
    }

    private void BindCharacterCore() {
        Container.Bind<BattlefieldRegistry>().AsSingle();

        Container.Bind<CharacterTeamService>().AsSingle();
        Container.Bind<CharacterAnchorService>().AsSingle();
        Container.Bind<CharacterFormationService>().AsSingle();
        Container.Bind<CharacterGroupService>().AsSingle();

        Container.Bind<DetectionService>().AsSingle();
        Container.Bind<UtilityAIService>().AsSingle();
        Container.Bind<CharacterLineOfSightService>().AsSingle();

        Container.Bind<CharacterDeathEventService>().AsSingle();
        Container.Bind<CharacterDeathPresentationService>().AsSingle();

        Container.Bind<CharacterLifecycleFactory>().AsSingle();
        Container.Bind<CharacterPool>().AsSingle();
        Container.Bind<CharacterFactory>().AsSingle();

        Container.Bind<CharacterDeploymentPositionService>().AsSingle();
        Container.Bind<CharacterDeploymentService>().AsSingle();
    }

    private void BindPresence() {
        Container.BindInterfacesAndSelfTo<CharacterPresenceEffectService>().AsSingle();

        Container.Bind<CharacterTeleportPresenceService>().AsSingle();
        Container.Bind<CharacterArcPresenceService>().AsSingle();
        Container.Bind<CharacterAirPresenceService>().AsSingle();
        Container.Bind<CharacterUndergroundPresenceService>().AsSingle();

        Container.Bind<CharacterCameraVisibilityService>().AsSingle();
        Container.Bind<CharacterNavMeshPathService>().AsSingle();
        Container.Bind<CharacterNavRouteService>().AsSingle();

        Container.Bind<CharacterOffCameraCandidateService>().AsSingle();
        Container.Bind<CharacterOffCameraPositionService>().AsSingle();
        Container.Bind<CharacterOffCameraFallbackService>().AsSingle();
        Container.Bind<CharacterOffCameraEnterService>().AsSingle();
        Container.Bind<CharacterOffCameraExitService>().AsSingle();
        Container.Bind<CharacterOffCameraRoutePresenceService>().AsSingle();

        Container.Bind<CharacterPresenceTransitionService>().AsSingle();
        Container.Bind<CharacterCombatPresenceService>().AsSingle();
    }
}