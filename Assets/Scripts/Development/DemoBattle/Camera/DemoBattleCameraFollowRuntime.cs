using UnityEngine;

public class DemoBattleCameraFollowRuntime {
    public Vector3 CameraOffset { get; set; }
    public Vector3 MoveVelocity { get; set; }
    public Vector3 CenterVelocity { get; set; }
    public Vector3 SmoothedCenter { get; set; }

    public float SmoothedRadius { get; set; }
    public float RadiusVelocity { get; set; }
    public float ZoomVelocity { get; set; }

    public bool HasOffset { get; set; }
    public bool HasSmoothedData { get; set; }
}