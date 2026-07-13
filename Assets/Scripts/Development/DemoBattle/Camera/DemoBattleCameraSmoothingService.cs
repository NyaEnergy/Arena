using UnityEngine;

public class DemoBattleCameraSmoothingService {
    private const float CENTER_SMOOTH_TIME = 0.35f;
    private const float MAX_CENTER_FOLLOW_SPEED = 8f;
    private const float RADIUS_SMOOTH_TIME = 0.35f;
    private const float MAX_RADIUS_CHANGE_SPEED = 8f;

    public void Initialize(DemoBattleCameraFollowRuntime runtime,
                           Vector3 center,
                           float radius) {
        runtime.SmoothedCenter = center;
        runtime.SmoothedRadius = radius;
        runtime.CenterVelocity = Vector3.zero;
        runtime.RadiusVelocity = 0f;
        runtime.HasSmoothedData = true;
    }

    public void Update(DemoBattleCameraFollowRuntime runtime,
                       Vector3 rawCenter,
                       float rawRadius) {
        if (!runtime.HasSmoothedData) {
            Initialize(runtime, rawCenter, rawRadius);
            return;
        }

        Vector3 centerVelocity = runtime.CenterVelocity;

        runtime.SmoothedCenter =
            Vector3.SmoothDamp(runtime.SmoothedCenter,
                               rawCenter,
                               ref centerVelocity,
                               CENTER_SMOOTH_TIME,
                               MAX_CENTER_FOLLOW_SPEED,
                               Time.deltaTime);

        runtime.CenterVelocity = centerVelocity;

        float radiusVelocity = runtime.RadiusVelocity;

        runtime.SmoothedRadius =
            Mathf.SmoothDamp(runtime.SmoothedRadius,
                             rawRadius,
                             ref radiusVelocity,
                             RADIUS_SMOOTH_TIME,
                             MAX_RADIUS_CHANGE_SPEED,
                             Time.deltaTime);

        runtime.RadiusVelocity = radiusVelocity;
    }
}