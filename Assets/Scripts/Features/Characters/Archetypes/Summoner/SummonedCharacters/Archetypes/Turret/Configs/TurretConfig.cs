using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Archetypes/Summoner/Turret Config",
                 fileName = "TurretConfig")]
public class TurretConfig : SummonedCharacterConfig,
                            ICharacterAttackConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 25f;
    [SerializeField] private float _moveSpeed;
    [SerializeField] private TurretView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Ranged;

    [Header("Presence")]
    [SerializeField] private CharacterPresencePresentationConfig _entryPresentation;
    [SerializeField] private CharacterPresencePresentationConfig _exitPresentation;

    [Header("Combat")]
    [SerializeField] private float _damage = 12f;
    [SerializeField] private float _attackCooldown = 1.1f;
    [SerializeField] private Range _attackDistanceRange = new(0f, 8f);
    [SerializeField] private LayerMask _lineOfSightBlockingLayers = ~0;
    [SerializeField] private QueryTriggerInteraction _lineOfSightTriggerInteraction = QueryTriggerInteraction.Ignore;

    public override SummonedCharacterType SummonedCharacterType => SummonedCharacterType.Turret;

    public override float MaxHP => _maxHP;
    public override float MoveSpeed => _moveSpeed;
    public override CharacterView Prefab => _prefab;
    public override CharacterCombatRow CombatRow => _combatRow;

    public override CharacterPresencePresentationConfig EntryPresentation => _entryPresentation;
    public override CharacterPresencePresentationConfig ExitPresentation => _exitPresentation;

    public float Damage => _damage;
    public float AttackCooldown => _attackCooldown;
    public Range AttackDistanceRange => _attackDistanceRange;
    public LayerMask LineOfSightBlockingLayers => _lineOfSightBlockingLayers;
    public QueryTriggerInteraction LineOfSightTriggerInteraction => _lineOfSightTriggerInteraction;
}