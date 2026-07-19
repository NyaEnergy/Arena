using UnityEngine;

public class ControllerRuntime {
    public float NextFieldTime { get; private set; }

    public void Reset() {
        NextFieldTime = float.NegativeInfinity;
    }

    public bool IsReady(float currentTime) {
        return currentTime >= NextFieldTime;
    }

    public void StartCooldown(float currentTime,
                              float cooldown) {

        NextFieldTime = currentTime + Mathf.Max(0f, cooldown);
    }
}