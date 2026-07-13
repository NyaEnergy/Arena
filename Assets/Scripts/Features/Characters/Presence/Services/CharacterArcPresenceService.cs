using UnityEngine;

public class CharacterArcPresenceService {
    private const float FULL_ROTATION = 360f;

    private readonly CharacterPresenceEffectService _effectService;

    public CharacterArcPresenceService(CharacterPresenceEffectService effectService) {
        _effectService = effectService;
    }

    public bool Begin(CharacterView view,
                      CharacterArcPresenceConfig config,
                      CharacterPresenceTransitionRuntime runtime,
                      CharacterPresenceTransitionRequest request) {
        if (view == null ||
            config == null ||
            runtime == null) {
            return true;
        }

        runtime.BeginArc(request, config);

        view.SetNavigationEnabled(false);
        view.transform.SetPositionAndRotation(request.StartPosition,
                                              request.StartRotation);

        return false;
    }

    public bool Tick(CharacterView view,
                     CharacterPresenceTransitionRuntime runtime) {

        CharacterArcPresenceRuntime arc = runtime?.ArcRuntime;

        if (view == null ||
            arc == null ||
            !arc.IsActive) return true;

        arc.Advance(Time.deltaTime);
        ApplyTransform(view, arc);

        if (arc.Progress < 1f) return false;

        Complete(view, runtime, arc);
        return true;
    }

    public void Cancel(CharacterView view,
                       CharacterPresenceTransitionRuntime runtime) {
        
        CharacterArcPresenceRuntime arc = runtime?.ArcRuntime;

        if (view == null ||
            arc == null ||
            !arc.IsActive) return;

        view.transform.SetPositionAndRotation(arc.EndPosition,
                                              arc.EndRotation);

        runtime.Reset();
    }

    private void ApplyTransform(CharacterView view,
                                CharacterArcPresenceRuntime arc) {
        
        float progress = arc.Progress;

        Vector3 position =
            Vector3.Lerp(arc.StartPosition,
                         arc.EndPosition,
                         progress);

        position.y += 4f *
                      arc.Height *
                      progress *
                      (1f - progress);

        Quaternion rotation =
            Quaternion.Slerp(arc.StartRotation,
                             arc.EndRotation,
                             progress);

        Quaternion spin =
            Quaternion.Euler(FULL_ROTATION *
                             arc.RotationCount *
                             progress,
                             0f,
                             0f);

        view.transform.SetPositionAndRotation(position, rotation * spin);
    }

    private void Complete(CharacterView view,
                          CharacterPresenceTransitionRuntime runtime,
                          CharacterArcPresenceRuntime arc) {
        view.transform.SetPositionAndRotation(arc.EndPosition,
                                              arc.EndRotation);

        if (arc.Direction == CharacterPresenceTransitionDirection.Enter) {
            view.SetNavigationEnabled(true);
        }

        _effectService.Play(arc.EffectPrefab,
                            arc.EndPosition,
                            arc.EndRotation);

        runtime.Complete();
    }
}