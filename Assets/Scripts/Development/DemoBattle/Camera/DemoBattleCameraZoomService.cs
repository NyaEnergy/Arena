using UnityEngine;

public class DemoBattleCameraZoomService {
    private const float MIN_RADIUS_FOR_ZOOM = 4f;
    private const float MAX_RADIUS_FOR_ZOOM = 13f;

    private const float MIN_ORTHOGRAPHIC_SIZE = 6f;
    private const float MAX_ORTHOGRAPHIC_SIZE = 13f;

    private const float MIN_FIELD_OF_VIEW = 45f;
    private const float MAX_FIELD_OF_VIEW = 68f;

    private const float ZOOM_SMOOTH_TIME = 0.3f;
    private const float MAX_ZOOM_SPEED = 8f;

    public void UpdateZoom(Camera camera,
                           float radius,
                           DemoBattleCameraFollowRuntime runtime) {
        float zoomFactor =
            Mathf.InverseLerp(
                MIN_RADIUS_FOR_ZOOM,
                MAX_RADIUS_FOR_ZOOM,
                radius);

        float zoomVelocity = runtime.ZoomVelocity;

        if (camera.orthographic) {
            float targetSize =
                Mathf.Lerp(
                    MIN_ORTHOGRAPHIC_SIZE,
                    MAX_ORTHOGRAPHIC_SIZE,
                    zoomFactor);

            camera.orthographicSize =
                Mathf.SmoothDamp(
                    camera.orthographicSize,
                    targetSize,
                    ref zoomVelocity,
                    ZOOM_SMOOTH_TIME,
                    MAX_ZOOM_SPEED,
                    Time.deltaTime);
        } else {
            float targetFieldOfView =
                Mathf.Lerp(
                    MIN_FIELD_OF_VIEW,
                    MAX_FIELD_OF_VIEW,
                    zoomFactor);

            camera.fieldOfView =
                Mathf.SmoothDamp(
                    camera.fieldOfView,
                    targetFieldOfView,
                    ref zoomVelocity,
                    ZOOM_SMOOTH_TIME,
                    MAX_ZOOM_SPEED,
                    Time.deltaTime);
        }

        runtime.ZoomVelocity = zoomVelocity;
    }
}