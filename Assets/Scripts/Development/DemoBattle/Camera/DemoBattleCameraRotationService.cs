using UnityEngine;

public class DemoBattleCameraRotationService {
    private const float MAX_ROTATION_SPEED = 120f;

    public void Rotate(Camera camera,
                       Vector3 focusPoint) {
        Transform cameraTransform = camera.transform;
        Vector3 lookDirection = focusPoint - cameraTransform.position;

        if (lookDirection.sqrMagnitude <= 0f) return;

        Quaternion targetRotation =
            Quaternion.LookRotation(
                lookDirection.normalized,
                Vector3.up);

        cameraTransform.rotation =
            Quaternion.RotateTowards(
                cameraTransform.rotation,
                targetRotation,
                MAX_ROTATION_SPEED *
                Time.deltaTime);
    }
}