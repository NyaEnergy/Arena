using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Presence/Arc",
                 fileName = "ArcPresence")]
public class CharacterArcPresenceConfig : CharacterPresencePresentationConfig {
    [SerializeField] private ParticleSystem _effectPrefab;
    [SerializeField] private float _duration = 0.65f;
    [SerializeField] private float _height = 2.5f;
    [SerializeField] private float _rotationCount = 1f;

    public ParticleSystem EffectPrefab => _effectPrefab;
    public float Duration => _duration;
    public float Height => _height;
    public float RotationCount => _rotationCount;
}