public class CharacterPresenceTransitionService {
    private readonly CharacterTeleportPresenceService _teleportService;

    private readonly CharacterArcPresenceService _arcService;

    private readonly CharacterAirPresenceService _airService;

    private readonly CharacterUndergroundPresenceService _undergroundService;

    private readonly CharacterOffCameraRoutePresenceService _offCameraService;

    public CharacterPresenceTransitionService(
            CharacterTeleportPresenceService teleportService,
            CharacterArcPresenceService arcService,
            CharacterAirPresenceService airService,
            CharacterUndergroundPresenceService undergroundService,
            CharacterOffCameraRoutePresenceService offCameraService) {

        _teleportService = teleportService;
        _arcService = arcService;
        _airService = airService;
        _undergroundService = undergroundService;
        _offCameraService = offCameraService;
    }

    public bool Begin(CharacterView view,
                      CharacterPresencePresentationConfig config,
                      CharacterPresenceTransitionRuntime runtime,
                      CharacterPresenceTransitionRequest request) {
        
        if (config == null) {
            ApplyInstant(view, request);
            return true;
        }

        if (config is CharacterTeleportPresenceConfig teleport) {
            return _teleportService.Begin(view, teleport, request);
        }

        if (config is CharacterArcPresenceConfig arc) {
            return _arcService.Begin(view, arc, runtime, request);
        }

        if (config is CharacterAirPresenceConfig air) {
            return _airService.Begin(view, air, runtime, request);
        }

        if (config is
            CharacterUndergroundPresenceConfig underground) {
            return _undergroundService.Begin(view, underground, runtime, request);
        }

        if (config is
            CharacterOffCameraRoutePresenceConfig offCamera) {
            return _offCameraService.Begin(view, offCamera, runtime, request);
        }

        ApplyInstant(view, request);
        return true;
    }

    public bool Tick(CharacterView view,
                     CharacterPresenceTransitionRuntime runtime) {

        if (runtime == null ||
            !runtime.IsActive) {
            return true;
        }

        if (runtime.Config is CharacterArcPresenceConfig) {
            return _arcService.Tick(view, runtime);
        }

        if (runtime.Config is CharacterAirPresenceConfig) {
            return _airService.Tick(view, runtime);
        }

        if (runtime.Config is CharacterUndergroundPresenceConfig) {
            return _undergroundService.Tick(view, runtime);
        }

        if (runtime.Config is CharacterOffCameraRoutePresenceConfig) {
            return _offCameraService.Tick(view, runtime);
        }

        runtime.Complete();
        return true;
    }

    public void Cancel(CharacterView view,
                       CharacterPresenceTransitionRuntime runtime) {

        if (runtime == null ||
            !runtime.IsActive) {
            return;
        }

        if (runtime.Config is CharacterArcPresenceConfig) {
            _arcService.Cancel(view, runtime);
            return;
        }

        if (runtime.Config is CharacterAirPresenceConfig) {
            _airService.Cancel(view, runtime);
            return;
        }

        if (runtime.Config is CharacterUndergroundPresenceConfig) {
            _undergroundService.Cancel(view, runtime);
            return;
        }

        if (runtime.Config is CharacterOffCameraRoutePresenceConfig) {
            _offCameraService.Cancel(view, runtime);
            return;
        }

        runtime.Reset();
    }

    private void ApplyInstant(CharacterView view,
                              CharacterPresenceTransitionRequest request) {

        if (view == null) return;

        view.transform.SetPositionAndRotation(request.EndPosition,
                                              request.EndRotation);

        if (request.Direction == CharacterPresenceTransitionDirection.Exit) {
            view.SetNavigationEnabled(false);
        }
    }
}