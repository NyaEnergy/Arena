using UnityEngine;

public readonly struct CharacterOffCameraPoint {
    public Vector3 Position { get; }
    public bool UsesTeleport { get; }

    public CharacterOffCameraPoint(Vector3 position,
                                   bool usesTeleport) {
        Position = position;
        UsesTeleport = usesTeleport;
    }
}