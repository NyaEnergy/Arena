using UnityEngine;

public readonly struct CharacterPresenceTransitionRequest {
    public readonly CharacterPresenceTransitionDirection Direction;

    public readonly Vector3 StartPosition;
    public readonly Vector3 EndPosition;

    public readonly Quaternion StartRotation;
    public readonly Quaternion EndRotation;

    public CharacterPresenceTransitionRequest(CharacterPresenceTransitionDirection direction,
                                              Vector3 startPosition,
                                              Vector3 endPosition,
                                              Quaternion startRotation,
                                              Quaternion endRotation) {
        Direction = direction;
        StartPosition = startPosition;
        EndPosition = endPosition;
        StartRotation = startRotation;
        EndRotation = endRotation;
    }
}