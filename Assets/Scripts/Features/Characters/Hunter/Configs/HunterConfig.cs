using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Characters/Hunter Config",
    fileName = "HunterConfig")]
public class HunterConfig : ScriptableObject, ICharacterConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 90f;
    [SerializeField] private float _moveSpeed = 4.5f;

    [Header("Ranged Combat")]
    [SerializeField] private Range _rangedAttackDistanceRange = new(2.5f, 7f);
    [SerializeField] private float _rangedDamage = 18f;
    [SerializeField] private float _rangedAttackCooldown = 0.8f;

    [Header("Melee Combat")]
    [SerializeField] private Range _meleeAttackDistanceRange = new(0f, 1.8f);
    [SerializeField] private float _meleeDamage = 22f;
    [SerializeField] private float _meleeAttackCooldown = 0.7f;

    [Header("Mode Switching")]
    [SerializeField] private Range _meleeModeSwitchRange = new(2.5f, 3.5f);

    [Header("Vanguard Formation")]
    [SerializeField] private float _vanguardSearchDistance = 8f;
    [SerializeField] private float _vanguardFollowDistance = 1.5f;
    [SerializeField] private float _vanguardSideOffset = 1.25f;

    [Header("Movement")]
    [SerializeField] private float _kiteSpeedMultiplier = 0.65f;

    [Header("Health Bar")]
    [SerializeField] private Color _healthBarBackgroundColor = new(0.04f, 0.04f, 0.04f, 0.85f);
    [SerializeField] private Color _healthBarFillColor = new(0.15f, 0.75f, 0.3f, 1f);

    [Header("Line Of Sight")]
    [SerializeField] private LayerMask _lineOfSightBlockingLayers = ~0;
    [SerializeField] private QueryTriggerInteraction _lineOfSightTriggerInteraction = QueryTriggerInteraction.Ignore;

    public CharacterType CharacterType => CharacterType.Hunter;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;

    public Range RangedAttackDistanceRange => _rangedAttackDistanceRange;
    public float RangedDamage => _rangedDamage;
    public float RangedAttackCooldown => _rangedAttackCooldown;

    public Range MeleeAttackDistanceRange => _meleeAttackDistanceRange;
    public float MeleeDamage => _meleeDamage;
    public float MeleeAttackCooldown => _meleeAttackCooldown;

    public Range MeleeModeSwitchRange => _meleeModeSwitchRange;

    public float VanguardSearchDistance => _vanguardSearchDistance;
    public float VanguardFollowDistance => _vanguardFollowDistance;
    public float VanguardSideOffset => _vanguardSideOffset;

    public float KiteSpeedMultiplier => _kiteSpeedMultiplier;

    public Color HealthBarBackgroundColor => _healthBarBackgroundColor;
    public Color HealthBarFillColor => _healthBarFillColor;

    public LayerMask LineOfSightBlockingLayers => _lineOfSightBlockingLayers;
    public QueryTriggerInteraction LineOfSightTriggerInteraction => _lineOfSightTriggerInteraction;
}