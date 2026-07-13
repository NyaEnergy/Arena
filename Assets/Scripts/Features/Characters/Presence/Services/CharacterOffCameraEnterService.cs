public class CharacterOffCameraEnterService {
    private readonly CharacterOffCameraPositionService _positionService;
    private readonly CharacterNavRouteService _routeService;
    private readonly CharacterOffCameraFallbackService _fallbackService;
    private readonly CharacterCameraVisibilityService _visibilityService;

    public CharacterOffCameraEnterService(
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
                request.EndPosition,
                CharacterPresenceTransitionDirection.Enter,
                out CharacterOffCameraPoint point)) {

            return Teleport(view, config, request,
                            request.EndPosition);
        }

        if (point.UsesTeleport) {
            return Teleport(view, config, request,
                            point.Position);
        }

        if (!_routeService.BeginAt(view, point.Position,
                                   request.EndPosition)) {

            return Teleport(view, config, request,
                request.EndPosition);
        }

        runtime.Begin(config, request,
                      request.EndPosition);

        return false;
    }

    public bool Tick(CharacterView view,
                     CharacterPresenceTransitionRuntime runtime) {

        if (_visibilityService.IsVisible(
                view, view.transform.position)) {

            return Complete(view, runtime);
        }

        CharacterNavRouteState state =
            _routeService.Tick(view);

        if (state == CharacterNavRouteState.Running) {
            return false;
        }

        if (state == CharacterNavRouteState.Failed) {

            CharacterOffCameraRoutePresenceConfig config =
                runtime.Config as CharacterOffCameraRoutePresenceConfig;

            _fallbackService.Play(view, config,
                                  runtime.Request,
                                  runtime.Request.EndPosition);
        }

        return Complete(view, runtime);
    }

    public void Cancel(CharacterView view,
                       CharacterPresenceTransitionRuntime runtime) {
        
        _routeService.Cancel(view);
        runtime?.Reset();
    }

    private bool Teleport(CharacterView view,
                          CharacterOffCameraRoutePresenceConfig config,
                          CharacterPresenceTransitionRequest request,
                          UnityEngine.Vector3 position) {
        
        _fallbackService.Play(view, config, request, position);

        return true;
    }

    private bool Complete(CharacterView view,
                          CharacterPresenceTransitionRuntime runtime) {
        
        _routeService.Cancel(view);
        runtime.Complete();

        return true;
    }
}