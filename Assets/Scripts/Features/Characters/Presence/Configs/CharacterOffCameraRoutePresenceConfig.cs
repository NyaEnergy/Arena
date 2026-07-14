using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Characters/Presence/Off Camera Route",
    fileName = "OffCameraRoutePresence")]
public class CharacterOffCameraRoutePresenceConfig : CharacterPresencePresentationConfig {
    
    [SerializeField] private CharacterTeleportPresenceConfig _fallbackTeleport;

    [SerializeField] private float _offCameraPadding = 1.5f;
    [SerializeField] private float _maxPathLength = 35f;
    [SerializeField] private Range _searchDistance = new(4f, 20f);

    public CharacterTeleportPresenceConfig FallbackTeleport => _fallbackTeleport;
    public float OffCameraPadding => _offCameraPadding;
    public float MaxPathLength => _maxPathLength;
    public Range SearchDistance => _searchDistance;
}