using UnityEngine;

public class TurretShotRuntime {
    public TurretView View { get; private set; }
    public float RemainingTime { get; private set; }
    public bool IsActive { get; private set; }

    public void Begin(TurretView view,
                      float duration) {
        View = view;
        RemainingTime = Mathf.Max(0f, duration);

        IsActive = true;
    }

    public void Advance(float deltaTime) {
        if (!IsActive) return;

        RemainingTime -= Mathf.Max(0f, deltaTime);
    }

    public void Reset() {
        View = null;
        RemainingTime = 0f;
        IsActive = false;
    }
}