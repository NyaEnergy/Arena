using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Character")]
public class CharacterConfig : ScriptableObject {
    public string Id;
    public float MaxHP;
    public float Damage;
    public float MoveSpeed;
    public float AttackRange;
    public float Accoracy;
    public float Dodge;
    public AbilityConfig[] Abilities;
}
