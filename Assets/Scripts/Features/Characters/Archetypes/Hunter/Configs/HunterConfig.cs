using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Archetypes/Hunter Config",
                 fileName = "HunterConfig")]
public class HunterConfig : ScriptableObject,
                            ICharacterConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 90f;
    [SerializeField] private float _moveSpeed = 4.5f;
    [SerializeField, Min(0.1f)] private float _threatWeight = 4f;
    [SerializeField] private HunterView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Ranged;

    [Header("Presence")]
    [SerializeField] private CharacterPresencePresentationConfig _entryPresentation;
    [SerializeField] private CharacterPresencePresentationConfig _exitPresentation;

    [Header("Ranged Combat")]
    [SerializeField] private float _rangedDamage = 18f;
    [SerializeField] private float _rangedAttackCooldown = 0.8f;
    [SerializeField] private Range _rangedAttackDistanceRange = new(2.5f, 7f);

    [Header("Melee Combat")]
    [SerializeField] private float _meleeDamage = 22f;
    [SerializeField] private float _meleeAttackCooldown = 0.7f;
    [SerializeField] private Range _meleeAttackDistanceRange = new(0f, 1.8f);

    [Header("Mode Switching")]
    [SerializeField] private Range _meleeModeSwitchRange = new(2.5f, 3.5f);

    [Header("Vanguard Formation")]
    [SerializeField] private float _vanguardSearchDistance = 8f;
    [SerializeField] private float _vanguardFollowDistance = 1.5f;
    [SerializeField] private float _vanguardSideOffset = 1.25f;

    [Header("Movement")]
    [SerializeField] private float _kiteSpeedMultiplier = 0.65f;

    [Header("Line Of Sight")]
    [SerializeField] private LayerMask _lineOfSightBlockingLayers = ~0;
    [SerializeField] private QueryTriggerInteraction _lineOfSightTriggerInteraction = QueryTriggerInteraction.Ignore;

    public CharacterType CharacterType => CharacterType.Hunter;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public float ThreatWeight => Mathf.Max(0.1f, _threatWeight);
    public CharacterView Prefab => _prefab;
    public CharacterCombatRow CombatRow => _combatRow;

    public CharacterPresencePresentationConfig EntryPresentation => _entryPresentation;
    public CharacterPresencePresentationConfig ExitPresentation => _exitPresentation;

    public float RangedDamage => _rangedDamage;
    public float RangedAttackCooldown => _rangedAttackCooldown;
    public Range RangedAttackDistanceRange => _rangedAttackDistanceRange;

    public float MeleeDamage => _meleeDamage;
    public float MeleeAttackCooldown => _meleeAttackCooldown;
    public Range MeleeAttackDistanceRange => _meleeAttackDistanceRange;

    public Range MeleeModeSwitchRange => _meleeModeSwitchRange;

    public float VanguardSearchDistance => _vanguardSearchDistance;
    public float VanguardFollowDistance => _vanguardFollowDistance;
    public float VanguardSideOffset => _vanguardSideOffset;

    public float KiteSpeedMultiplier => _kiteSpeedMultiplier;

    public LayerMask LineOfSightBlockingLayers => _lineOfSightBlockingLayers;
    public QueryTriggerInteraction LineOfSightTriggerInteraction => _lineOfSightTriggerInteraction;
}