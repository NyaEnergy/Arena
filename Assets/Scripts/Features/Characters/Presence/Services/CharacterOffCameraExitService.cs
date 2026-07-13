public class CharacterOffCameraExitService {
    private readonly CharacterOffCameraPositionService _positionService;
    private readonly CharacterNavRouteService _routeService;
    private readonly CharacterOffCameraFallbackService _fallbackService;
    private readonly CharacterCameraVisibilityService _visibilityService;

    public CharacterOffCameraExitService(
                CharacterOffCameraPositionService positionService,
                CharacterNavRouteService routeService,
                CharacterOffCameraFallbackService fallbackService,
                CharacterCameraVisibilityService visibilityService) {
        
        _positionService = positionService;
        _routeService = routeService;
        _fallbackService = fallbackService;
        _visibilityService = visibilityService;
    }

    public bool Begin(CharacterView view,
            CharacterOffCameraRoutePresenceConfig config,
            CharacterPresenceTransitionRuntime runtime,
            CharacterPresenceTransitionRequest request) {
        
        if (!_positionService.TryGet(
                view, config,
                request.StartPosition,
                CharacterPresenceTransitionDirection.Exit,
                out CharacterOffCameraPoint point)) {
            return TeleportCurrent(
                view, config, request);
        }

        if (point.UsesTeleport) {
            return TeleportCurrent(
                view, config, request);
        }

        if (!_routeService.BeginFromCurrent(
                view, point.Position)) {
            return TeleportCurrent(
                view, config, request);
        }

        runtime.Begin(config, request,
                      point.Position);

        return false;
    }

    public bool Tick(CharacterView view,
                     CharacterPresenceTransitionRuntime runtime) {
        if (!_visibilityService.IsVisible(
                view, view.transform.position)) {
            return Complete(view, runtime);
        }

        CharacterNavRouteState state =
            _routeService.Tick(view);

        if (state == CharacterNavRouteState.Running) {
            return false;
        }

        if (state == CharacterNavRouteState.Failed ||
            _visibilityService.IsVisible(
                view, view.transform.position)) {

            CharacterOffCameraRoutePresenceConfig config =
                runtime.Config as CharacterOffCameraRoutePresenceConfig;

            _fallbackService.Play(
                view, config, runtime.Request,
                view.transform.position);
        }

        return Complete(view, runtime);
    }

    public void Cancel(CharacterView view,
                       CharacterPresenceTransitionRuntime runtime) {
        _routeService.Cancel(view);
        runtime?.Reset();
    }

    private bool TeleportCurrent(CharacterView view,
                                 CharacterOffCameraRoutePresenceConfig config,
                                 CharacterPresenceTransitionRequest request) {
        _fallbackService.Play(
            view, config, request,
            view.transform.position);

        return true;
    }

    private bool Complete(CharacterView view,
                          CharacterPresenceTransitionRuntime runtime) {

        _routeService.Cancel(view);
        view.SetNavigationEnabled(false);
        runtime.Complete();
        return true;
    }
}