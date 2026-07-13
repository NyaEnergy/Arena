public class CharacterOffCameraRoutePresenceService {
    private readonly CharacterOffCameraEnterService _enterService;
    private readonly CharacterOffCameraExitService _exitService;

    public CharacterOffCameraRoutePresenceService(
                CharacterOffCameraEnterService enterService,
                CharacterOffCameraExitService exitService) {

        _enterService = enterService;
        _exitService = exitService;
    }

    public bool Begin(CharacterView view,
                      CharacterOffCameraRoutePresenceConfig config,
                      CharacterPresenceTransitionRuntime runtime,
                      CharacterPresenceTransitionRequest request) {
        if (request.Direction == CharacterPresenceTransitionDirection.Enter) {

            return _enterService.Begin(
                view, config, runtime, request);
        }

        return _exitService.Begin(
            view, config, runtime, request);
    }

    public bool Tick(CharacterView view,
                     CharacterPresenceTransitionRuntime runtime) {

        if (runtime.Request.Direction == CharacterPresenceTransitionDirection.Enter) {
            return _enterService.Tick(view, runtime);
        }

        return _exitService.Tick(view, runtime);
    }

    public void Cancel(CharacterView view,
                       CharacterPresenceTransitionRuntime runtime) {

        if (runtime == null ||
           !runtime.IsActive) {
            return;
        }

        if (runtime.Request.Direction == CharacterPresenceTransitionDirection.Enter) {
            _enterService.Cancel(view, runtime);

            return;
        }

        _exitService.Cancel(view, runtime);
    }
}