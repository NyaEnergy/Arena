using UnityEngine;

public class CharacterDeathRuntime {
    public float RemainingTime { get; private set; }
    public bool IsActive { get; private set; }

    public bool IsComplete => IsActive &&
                              RemainingTime <= 0f;

    public void Begin(float duration) {
        RemainingTime = Mathf.Max(0f, duration);
        IsActive = true;
    }

    public void Advance(float deltaTime) {
        if (!IsActive) return;

        RemainingTime -= Mathf.Max(0f, deltaTime);
    }

    public void Reset() {
        RemainingTime = 0f;
        IsActive = false;
    }
}