using UnityEngine;

public class CharacterUndergroundPresenceService {
    private readonly CharacterPresenceEffectService _effectService;

    public CharacterUndergroundPresenceService(CharacterPresenceEffectService effectService) {
        _effectService = effectService;
    }

    public bool Begin(CharacterView view,
                      CharacterUndergroundPresenceConfig config,
                      CharacterPresenceTransitionRuntime runtime,
                      CharacterPresenceTransitionRequest request) {
        if (view == null ||
            config == null ||
            runtime == null) {
            return true;
        }

        runtime.BeginUnderground(request, config);

        CharacterUndergroundPresenceRuntime underground =
            runtime.UndergroundRuntime;

        view.SetNavigationEnabled(false);
        view.transform.SetPositionAndRotation(
            underground.StartPosition,
            underground.StartRotation
        );

        return false;
    }

    public bool Tick(CharacterView view,
                     CharacterPresenceTransitionRuntime runtime) {
        
        CharacterUndergroundPresenceRuntime underground =
            runtime?.UndergroundRuntime;

        if (view == null ||
            underground == null ||
            !underground.IsActive) return true;

        underground.Advance(Time.deltaTime);

        float progress =
            Mathf.SmoothStep(
                0f, 1f, underground.Progress);

        view.transform.SetPositionAndRotation(
            Vector3.Lerp(underground.StartPosition,
                         underground.EndPosition,
                         progress),
            Quaternion.Slerp(underground.StartRotation,
                             underground.EndRotation,
                             progress)
        );

        if (underground.Progress < 1f) return false;

        Complete(view, runtime, underground);
        return true;
    }

    public void Cancel(CharacterView view,
                       CharacterPresenceTransitionRuntime runtime) {

        CharacterUndergroundPresenceRuntime underground =
            runtime?.UndergroundRuntime;

        if (view == null ||
            underground == null ||
           !underground.IsActive) return;

        view.transform.SetPositionAndRotation(
            underground.EndPosition,
            underground.EndRotation
        );

        runtime.Reset();
    }

    private void Complete(CharacterView view,
                          CharacterPresenceTransitionRuntime runtime,
                          CharacterUndergroundPresenceRuntime underground) {
        view.transform.SetPositionAndRotation(underground.EndPosition,
                                              underground.EndRotation);

        if (underground.Direction == CharacterPresenceTransitionDirection.Enter) {
            view.SetNavigationEnabled(true);
        }

        _effectService.Play(underground.EffectPrefab,
                            underground.EndPosition,
                            underground.EndRotation);

        runtime.Complete();
    }
}