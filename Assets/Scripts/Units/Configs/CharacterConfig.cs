using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Units/Character Config", fileName = "CharacterConfig")]
public class CharacterConfig : ScriptableObject {
    [SerializeField] private BattlefieldCharacter _prefab;
    
    [SerializeField] private TeamType _teamType;
    [SerializeField] private CharacterType _characterType;

    [SerializeField] private float _maxHP = 125f;
    [SerializeField] private float _damage = 18f;
    [SerializeField] private float _moveSpeed = 4.2f;
    [SerializeField] private float _attackRange = 2.1f;
    [SerializeField] private float _attackCooldown = 1.4f;
    [SerializeField] private float _accuracy = 88f;
    [SerializeField] private float _dodge = 12f;
    [SerializeField] private float _critChance = 0.12f;
    [SerializeField] private float _critMultiplier = 1.7f;

    public BattlefieldCharacter Prefab => _prefab;

    public TeamType TeamType => _teamType;
    public CharacterType CharacterType => _characterType;

    public float MaxHP => _maxHP;
    public float Damage => _damage;
    public float MoveSpeed => _moveSpeed;
    public float AttackRange => _attackRange;
    public float AttackCooldown => _attackCooldown;
    public float Accuracy => _accuracy;
    public float Dodge => _dodge;
    public float CritChance => _critChance;
    public float CritMultiplier => _critMultiplier;
}
