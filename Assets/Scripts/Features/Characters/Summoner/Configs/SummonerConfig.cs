using UnityEngine;

[CreateAssetMenu(
    menuName = "Configs/Characters/Summoner Config",
    fileName = "SummonerConfig")]
public class SummonerConfig : ScriptableObject,
                              ICharacterConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 95f;
    [SerializeField] private float _moveSpeed = 4.1f;
    [SerializeField] private SummonerView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Ranged;
    [SerializeField] private CharacterPresencePresentationSettings _spawnPresentation;

    [Header("Summoning")]
    [SerializeField] private MinionConfig _minionConfig;
    [SerializeField] private float _summonCooldown = 3f;
    [SerializeField] private int _maxMinions = 3;
    [SerializeField] private Range _summonDistanceRange = new(4f, 8f);

    [Header("Minion Spawn")]
    [SerializeField] private float _minionForwardOffset = 1.6f;
    [SerializeField] private float _minionSideOffset = 0.8f;

    [Header("Movement")]
    [SerializeField] private float _retreatStepDistance = 2f;

    [Header("Health Bar")]
    [SerializeField] private Color _healthBarBackgroundColor = new(0.04f, 0.04f, 0.04f, 0.85f);
    [SerializeField] private Color _healthBarFillColor = new(0.15f, 0.75f, 0.3f, 1f);

    public CharacterType CharacterType => CharacterType.Summoner;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public CharacterView Prefab => _prefab;
    public CharacterCombatRow CombatRow => _combatRow;
    public CharacterPresencePresentationSettings SpawnPresentation => _spawnPresentation;

    public MinionConfig MinionConfig => _minionConfig;
    public float SummonCooldown => _summonCooldown;
    public int MaxMinions => _maxMinions;
    public Range SummonDistanceRange => _summonDistanceRange;

    public float MinionForwardOffset => _minionForwardOffset;
    public float MinionSideOffset => _minionSideOffset;

    public float RetreatStepDistance => _retreatStepDistance;

    public Color HealthBarBackgroundColor => _healthBarBackgroundColor;
    public Color HealthBarFillColor => _healthBarFillColor;
}