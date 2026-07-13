using UnityEngine;

public class CharacterAirPresenceService {
    private readonly CharacterPresenceEffectService _effectService;

    public CharacterAirPresenceService(CharacterPresenceEffectService effectService) {
        _effectService = effectService;
    }

    public bool Begin(CharacterView view,
                      CharacterAirPresenceConfig config,
                      CharacterPresenceTransitionRuntime runtime,
                      CharacterPresenceTransitionRequest request) {
        if (view == null ||
            config == null ||
            runtime == null) return true;

        runtime.BeginAir(request, config);

        CharacterAirPresenceRuntime air = runtime.AirRuntime;

        view.SetNavigationEnabled(false);
        view.transform.SetPositionAndRotation(
            air.StartPosition,
            air.StartRotation);

        return false;
    }

    public bool Tick(CharacterView view,
                     CharacterPresenceTransitionRuntime runtime) {
        CharacterAirPresenceRuntime air = runtime?.AirRuntime;

        if (view == null ||
            air == null ||
            !air.IsActive) return true;

        air.Advance(Time.deltaTime);

        float progress =
            Mathf.SmoothStep(
                0f, 1f, air.Progress);

        view.transform.SetPositionAndRotation(
            Vector3.Lerp(air.StartPosition,
                         air.EndPosition,
                         progress),
            Quaternion.Slerp(air.StartRotation,
                             air.EndRotation,
                             progress)
        );

        if (air.Progress < 1f) return false;

        Complete(view, runtime, air);
        return true;
    }

    public void Cancel(CharacterView view,
                       CharacterPresenceTransitionRuntime runtime) {
        CharacterAirPresenceRuntime air = runtime?.AirRuntime;

        if (view == null ||
            air == null ||
            !air.IsActive) return;

        view.transform.SetPositionAndRotation(
            air.EndPosition, air.EndRotation);

        runtime.Reset();
    }

    private void Complete(CharacterView view,
                          CharacterPresenceTransitionRuntime runtime,
                          CharacterAirPresenceRuntime air) {
        view.transform
            .SetPositionAndRotation(air.EndPosition,
                                    air.EndRotation);

        if (air.Direction == CharacterPresenceTransitionDirection.Enter) {
            view.SetNavigationEnabled(true);
        }

        _effectService.Play(air.EffectPrefab,
                            air.EndPosition,
                            air.EndRotation);

        runtime.Complete();
    }
}