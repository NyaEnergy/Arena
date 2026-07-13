using UnityEngine;

public class CharacterOffCameraPositionService {
    private const int DIRECTION_COUNT = 16;
    private const float SEARCH_STEP = 1f;

    private readonly Camera _camera;

    private readonly CharacterOffCameraCandidateService _candidateService;
    private readonly CharacterCameraVisibilityService _visibilityService;

    public CharacterOffCameraPositionService(Camera camera,
            CharacterOffCameraCandidateService candidateService,
            CharacterCameraVisibilityService visibilityService) {
        
        _camera = camera;
        _candidateService = candidateService;
        _visibilityService = visibilityService;
    }

    public bool TryGet(CharacterView view,
            CharacterOffCameraRoutePresenceConfig config,
            Vector3 anchor,
            CharacterPresenceTransitionDirection direction,
            out CharacterOffCameraPoint point) {
        
        point = default;

        if (view == null ||
            config == null ||
            _camera == null) return false;

        Vector3 preferred = anchor -
                            _camera.transform.position;

        preferred.y = 0f;

        if (preferred.sqrMagnitude < 0.001f) {
            preferred = Vector3.forward;
        }

        preferred.Normalize();

        float minimum = Mathf.Max(0f, config.MinSearchDistance);

        float maximum = Mathf.Max(minimum, config.MaxSearchDistance);

        bool hasFallback = false;
        Vector3 fallback = anchor;
        float fallbackDistance = -1f;

        for (float distance = maximum; distance >= minimum; distance -= SEARCH_STEP) {
            
            for (int i = 0; i < DIRECTION_COUNT; i++) {

                Vector3 directionVector = Rotate(preferred, i);

                Vector3 candidate = anchor +
                                    directionVector *
                                    (distance + config.OffCameraPadding);

                if (!_candidateService.TryGet(
                        config,
                        candidate,
                        anchor,
                        direction,
                        out Vector3 position)) continue;

                float sqrDistance = Vector3.SqrMagnitude(position - anchor);

                if (sqrDistance > fallbackDistance) {
                    fallback = position;
                    fallbackDistance = sqrDistance;
                    hasFallback = true;
                }

                if (!_visibilityService.IsVisible(view, position)) {
                    point = new CharacterOffCameraPoint(position, false);
                    return true;
                }
            }
        }

        if (!hasFallback) return false;

        point = new CharacterOffCameraPoint(fallback, true);
        return true;
    }

    private Vector3 Rotate(Vector3 direction,
                           int index) {
        float angle = 360f / DIRECTION_COUNT * index;

        return Quaternion.AngleAxis(
            angle, Vector3.up) * direction;
    }
}