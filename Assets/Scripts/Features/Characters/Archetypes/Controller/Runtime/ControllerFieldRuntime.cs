using System.Collections.Generic;
using UnityEngine;

public class ControllerFieldRuntime {
    private readonly HashSet<CharacterBrain> _reportedTargets = new();

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

    public bool MarkAffected(CharacterBrain target) {
        return target != null &&
               _reportedTargets.Add(target);
    }
}