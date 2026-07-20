using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Archetypes/Summoner/Minion Config",
                 fileName = "MinionConfig")]
public class MinionConfig : SummonedCharacterConfig,
                            ICharacterAttackConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 35f;
    [SerializeField] private float _moveSpeed = 5.2f;
    [SerializeField, Min(0.1f)] private float _threatWeight = 1f;
    [SerializeField] private MinionView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Front;

    [Header("Presence")]
    [SerializeField] private CharacterPresencePresentationConfig _entryPresentation;
    [SerializeField] private CharacterPresencePresentationConfig _exitPresentation;

    [Header("Combat")]
    [SerializeField] private float _damage = 8f;
    [SerializeField] private float _attackCooldown = 0.9f;
    [SerializeField] private Range _attackDistanceRange = new(0f, 1.3f);

    public override SummonedCharacterType SummonedCharacterType => SummonedCharacterType.Minion;

    public override float MaxHP => _maxHP;
    public override float MoveSpeed => _moveSpeed;
    public override float ThreatWeight => Mathf.Max(0.1f, _threatWeight);
    public override CharacterView Prefab => _prefab;
    public override CharacterCombatRow CombatRow => _combatRow;

    public override CharacterPresencePresentationConfig EntryPresentation => _entryPresentation;
    public override CharacterPresencePresentationConfig ExitPresentation => _exitPresentation;

    public float Damage => _damage;
    public float AttackCooldown => _attackCooldown;
    public Range AttackDistanceRange => _attackDistanceRange;
}