using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Presence/Air",
                 fileName = "AirPresence")]
public class CharacterAirPresenceConfig : CharacterPresencePresentationConfig {
    [SerializeField] private ParticleSystem _effectPrefab;
    [SerializeField] private float _duration = 0.75f;
    [SerializeField] private float _height = 6f;

    public ParticleSystem EffectPrefab => _effectPrefab;
    public float Duration => _duration;
    public float Height => _height;
}