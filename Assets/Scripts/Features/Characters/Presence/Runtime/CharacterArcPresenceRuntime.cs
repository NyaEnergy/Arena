using UnityEngine;

public class CharacterArcPresenceRuntime {
    public CharacterPresenceTransitionDirection Direction { get; private set; }

    public Vector3 StartPosition { get; private set; }
    public Vector3 EndPosition { get; private set; }

    public Quaternion StartRotation { get; private set; }
    public Quaternion EndRotation { get; private set; }

    public ParticleSystem EffectPrefab { get; private set; }

    public float Duration { get; private set; }
    public float Height { get; private set; }
    public float RotationCount { get; private set; }
    public float ElapsedTime { get; private set; }

    public bool IsActive { get; private set; }

    public float Progress => Duration <= 0f ?
                             1f : Mathf.Clamp01(ElapsedTime / Duration);

    public void Begin(CharacterPresenceTransitionRequest request,
                      CharacterArcPresenceConfig config) {
        Direction = request.Direction;
        StartPosition = request.StartPosition;
        EndPosition = request.EndPosition;
        StartRotation = request.StartRotation;
        EndRotation = request.EndRotation;

        EffectPrefab = config.EffectPrefab;
        Duration = Mathf.Max(0.05f, config.Duration);
        Height = Mathf.Max(0f, config.Height);
        RotationCount = Mathf.Max(0f, config.RotationCount);

        ElapsedTime = 0f;
        IsActive = true;
    }

    public void Advance(float deltaTime) {
        ElapsedTime += Mathf.Max(0f, deltaTime);
    }

    public void Reset() {
        Direction = CharacterPresenceTransitionDirection.Enter;

        StartPosition = Vector3.zero;
        EndPosition = Vector3.zero;
        StartRotation = Quaternion.identity;
        EndRotation = Quaternion.identity;

        EffectPrefab = null;
        Duration = 0f;
        Height = 0f;
        RotationCount = 0f;
        ElapsedTime = 0f;
        IsActive = false;
    }
}