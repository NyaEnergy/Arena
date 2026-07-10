using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Characters/Minion Config",
    fileName = "MinionConfig")]
public class MinionConfig : ScriptableObject,
                            ICharacterConfig,
                            ICharacterAttackConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 35f;
    [SerializeField] private float _moveSpeed = 5.2f;
    [SerializeField] private MinionView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Front;
    [SerializeField] private CharacterPresencePresentationSettings _spawnPresentation;

    [Header("Combat")]
    [SerializeField] private float _damage = 8f;
    [SerializeField] private float _attackCooldown = 0.9f;
    [SerializeField] private Range _attackDistanceRange = new(0f, 1.3f);

    [Header("Health Bar")]
    [SerializeField] private Color _healthBarBackgroundColor = new(0.04f, 0.04f, 0.04f, 0.85f);
    [SerializeField] private Color _healthBarFillColor = new(0.15f, 0.75f, 0.3f, 1f);

    public CharacterType CharacterType => CharacterType.Minion;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public CharacterView Prefab => _prefab;
    public CharacterCombatRow CombatRow => _combatRow;
    public CharacterPresencePresentationSettings SpawnPresentation => _spawnPresentation;

    public float Damage => _damage;
    public float AttackCooldown => _attackCooldown;
    public Range AttackDistanceRange => _attackDistanceRange;

    public Color HealthBarBackgroundColor => _healthBarBackgroundColor;
    public Color HealthBarFillColor => _healthBarFillColor;
}