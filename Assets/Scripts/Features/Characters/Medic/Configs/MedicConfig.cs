using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Characters/Medic Config",
    fileName = "MedicConfig")]
public class MedicConfig : ScriptableObject,
                           ICharacterConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 110f;
    [SerializeField] private float _moveSpeed = 4.6f;
    [SerializeField] private MedicView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Support;
    [SerializeField] private CharacterPresencePresentationSettings _spawnPresentation;

    [Header("Combat")]
    [SerializeField] private float _damage = 16f;
    [SerializeField] private float _attackCooldown = 1.5f;
    [SerializeField] private Range _attackDistanceRange = new(0f, 9f);

    [Header("Healing")]
    [SerializeField] private float _healingPerSecond = 20f;
    [SerializeField] private float _healingDistance = 3f;
    [SerializeField] private float _supportDistance = 1.5f;
    [SerializeField] private float _emergencySwitchDelta = 0.05f;

    [SerializeField] private Range _emergencyHealthRange = new(0.15f, 0.35f);
    [SerializeField] private Range _criticalHealthRange = new(0.4f, 0.65f);

    [Header("Health Bar")]
    [SerializeField] private Color _healthBarBackgroundColor = new(0.04f, 0.04f, 0.04f, 0.85f);
    [SerializeField] private Color _healthBarFillColor = new(0.15f, 0.75f, 0.3f, 1f);

    [Header("Line Of Sight")]
    [SerializeField] private LayerMask _lineOfSightBlockingLayers = ~0;
    [SerializeField] private QueryTriggerInteraction _lineOfSightTriggerInteraction = QueryTriggerInteraction.Ignore;

    public CharacterType CharacterType => CharacterType.Medic;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public CharacterView Prefab => _prefab;
    public CharacterCombatRow CombatRow => _combatRow;
    public CharacterPresencePresentationSettings SpawnPresentation => _spawnPresentation;

    public float Damage => _damage;
    public float AttackCooldown => _attackCooldown;
    public Range AttackDistanceRange => _attackDistanceRange;

    public float HealingPerSecond => _healingPerSecond;
    public float HealingDistance => _healingDistance;
    public float SupportDistance => _supportDistance;
    public float EmergencySwitchDelta => _emergencySwitchDelta;

    public Range EmergencyHealthRange => _emergencyHealthRange;
    public Range CriticalHealthRange => _criticalHealthRange;

    public Color HealthBarBackgroundColor => _healthBarBackgroundColor;
    public Color HealthBarFillColor => _healthBarFillColor;

    public LayerMask LineOfSightBlockingLayers => _lineOfSightBlockingLayers;
    public QueryTriggerInteraction LineOfSightTriggerInteraction => _lineOfSightTriggerInteraction;
}