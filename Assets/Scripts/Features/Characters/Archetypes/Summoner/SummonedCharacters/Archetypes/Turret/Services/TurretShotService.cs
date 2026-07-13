using UnityEngine;

public class TurretShotService {
    private const float SHOT_DURATION = 0.08f;

    public void Play(TurretView view,
                     CharacterView target,
                     TurretShotRuntime runtime) {

        if (view == null ||
            target == null ||
            runtime == null) {
            return;
        }

        Reset(runtime);
        view.ShowShot(target.AimPosition);
        runtime.Begin(view, SHOT_DURATION);
    }

    public void Tick(TurretShotRuntime runtime) {
        if (runtime == null ||
            !runtime.IsActive)
                return;

        runtime.Advance(Time.deltaTime);

        if (runtime.RemainingTime > 0f)
            return;

        Reset(runtime);
    }

    public void Reset(TurretShotRuntime runtime) {
        if (runtime == null) return;

        runtime.View?.HideShot();
        runtime.Reset();
    }
}