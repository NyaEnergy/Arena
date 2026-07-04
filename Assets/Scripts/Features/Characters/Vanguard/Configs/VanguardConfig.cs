using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Characters/Vanguard Config",
    fileName = "VanguardConfig")]
public class VanguardConfig : ScriptableObject, ICharacterConfig, ICharacterAttackConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 180f;
    [SerializeField] private float _moveSpeed = 4f;

    [Header("Combat")]
    [SerializeField] private float _damage = 14f;
    [SerializeField] private Range _attackDistanceRange = new(0f, 2.1f);
    [SerializeField] private float _attackCooldown = 1f;

    [Header("Health Bar")]
    [SerializeField] private Color _healthBarBackgroundColor = new(0.04f, 0.04f, 0.04f, 0.85f);
    [SerializeField] private Color _healthBarFillColor = new(0.15f, 0.75f, 0.3f, 1f);

    public CharacterType CharacterType => CharacterType.Vanguard;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;

    public float Damage => _damage;
    public Range AttackDistanceRange => _attackDistanceRange;
    public float AttackCooldown => _attackCooldown;

    public Color HealthBarBackgroundColor => _healthBarBackgroundColor;
    public Color HealthBarFillColor => _healthBarFillColor;
}