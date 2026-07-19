using UnityEngine;

public class ControllerFieldRuntime {
    public TeamType TeamType { get; }
    public Vector3 Position { get; }
    public float Radius { get; }
    public float SlowMultiplier { get; }
    public ControllerFieldView View { get; }

    public float RemainingTime { get; private set; }

    public ControllerFieldRuntime(TeamType teamType,
                                  Vector3 position,
                                  float radius,
                                  float slowMultiplier,
                                  float duration,
                                  ControllerFieldView view) {

        TeamType = teamType;
        Position = position;
        Radius = radius;
        SlowMultiplier = slowMultiplier;
        RemainingTime = duration;
        View = view;
    }

    public bool Tick(float deltaTime) {
        RemainingTime -= Mathf.Max(0f, deltaTime);
        return RemainingTime > 0f;
    }
}