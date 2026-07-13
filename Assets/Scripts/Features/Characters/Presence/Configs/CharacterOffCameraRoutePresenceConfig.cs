using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Characters/Presence/Off Camera Route",
    fileName = "OffCameraRoutePresence")]
public class CharacterOffCameraRoutePresenceConfig : CharacterPresencePresentationConfig {
    
    [SerializeField] private CharacterTeleportPresenceConfig _fallbackTeleport;

    [SerializeField] private float _offCameraPadding = 1.5f;
    [SerializeField] private float _maxSearchDistance = 20f;
    [SerializeField] private float _minSearchDistance = 4f;
    [SerializeField] private float _maxPathLength = 35f;

    public CharacterTeleportPresenceConfig FallbackTeleport => _fallbackTeleport;
    public float OffCameraPadding => _offCameraPadding;
    public float MaxSearchDistance => _maxSearchDistance;
    public float MinSearchDistance => _minSearchDistance;
    public float MaxPathLength => _maxPathLength;
}