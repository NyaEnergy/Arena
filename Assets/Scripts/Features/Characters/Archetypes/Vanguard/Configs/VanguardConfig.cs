using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Archetypes/Vanguard Config",
                 fileName = "VanguardConfig")]
public class VanguardConfig : ScriptableObject,
                              ICharacterConfig,
                              ICharacterAttackConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 180f;
    [SerializeField] private float _moveSpeed = 4f;
    [SerializeField, Min(0.1f)] private float _threatWeight = 3f;
    [SerializeField] private VanguardView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Front;

    [Header("Presence")]
    [SerializeField] private CharacterPresencePresentationConfig _entryPresentation;
    [SerializeField] private CharacterPresencePresentationConfig _exitPresentation;

    [Header("Combat")]
    [SerializeField] private float _damage = 14f;
    [SerializeField] private float _attackCooldown = 1f;
    [SerializeField] private Range _attackDistanceRange = new(0f, 2.1f);

    public CharacterType CharacterType => CharacterType.Vanguard;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public float ThreatWeight => Mathf.Max(0.1f, _threatWeight);
    public CharacterView Prefab => _prefab;
    public CharacterCombatRow CombatRow => _combatRow;

    public CharacterPresencePresentationConfig EntryPresentation => _entryPresentation;
    public CharacterPresencePresentationConfig ExitPresentation => _exitPresentation;

    public float Damage => _damage;
    public float AttackCooldown => _attackCooldown;
    public Range AttackDistanceRange => _attackDistanceRange;
}