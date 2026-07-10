using UnityEngine;

[System.Serializable]
public class CharacterPresencePresentationSettings {
    [Header("Presence")]
    [SerializeField] private CharacterPresencePresentationType _presenceType =
                             CharacterPresencePresentationType.Instant;

    [Header("Effects")]
    [SerializeField] private ParticleSystem _enterEffectPrefab;
    [SerializeField] private ParticleSystem _exitEffectPrefab;

    [Header("Timing")]
    [SerializeField] private float _presentationDuration = 0.5f;

    [Header("Air")]
    [SerializeField] private float _airHeight = 6f;

    [Header("Off Camera Route")]
    [SerializeField] private float _offCameraPadding = 1.5f;
    [SerializeField] private float _maxEntrySearchDistance = 20f;
    [SerializeField] private float _minDistanceFromPressurePosition = 4f;
    [SerializeField] private float _maxPathLength = 35f;

    public CharacterPresencePresentationType PresenceType => _presenceType;

    public ParticleSystem EnterEffectPrefab => _enterEffectPrefab;
    public ParticleSystem ExitEffectPrefab => _exitEffectPrefab;

    public float PresentationDuration => _presentationDuration;
    public float AirHeight => _airHeight;

    public float OffCameraPadding => _offCameraPadding;
    public float MaxEntrySearchDistance => _maxEntrySearchDistance;
    public float MinDistanceFromPressurePosition => _minDistanceFromPressurePosition;
    public float MaxPathLength => _maxPathLength;
}