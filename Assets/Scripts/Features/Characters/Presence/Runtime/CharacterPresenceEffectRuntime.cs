using UnityEngine;

public class CharacterPresenceEffectRuntime {
    public ParticleSystem Prefab { get; }
    public ParticleSystem Instance { get; }
    public float RemainingTime { get; set; }

    public CharacterPresenceEffectRuntime(ParticleSystem prefab,
                                          ParticleSystem instance,
                                          float remainingTime) {
        Prefab = prefab;
        Instance = instance;
        RemainingTime = remainingTime;
    }
}
