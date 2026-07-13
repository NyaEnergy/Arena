using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Presence/Teleport",
                 fileName = "TeleportPresence")]
public class CharacterTeleportPresenceConfig : CharacterPresencePresentationConfig {
    [SerializeField] private ParticleSystem _effectPrefab;
    public ParticleSystem EffectPrefab => _effectPrefab;
}