using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Presence/Underground",
                 fileName = "UndergroundPresence")]
public class CharacterUndergroundPresenceConfig : CharacterPresencePresentationConfig {
    [SerializeField] private ParticleSystem _effectPrefab;
    [SerializeField] private float _duration = 0.7f;
    [SerializeField] private float _depth = 2f;

    public ParticleSystem EffectPrefab => _effectPrefab;
    public float Duration => _duration;
    public float Depth => _depth;
}