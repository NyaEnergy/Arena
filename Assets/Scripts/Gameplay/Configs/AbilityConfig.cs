using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Ability")]
public class AbilityConfig : ScriptableObject {
    public string Id;
    public float Cooldown;
    public float Range;
    public float Damage;
    public float CastTime;
}