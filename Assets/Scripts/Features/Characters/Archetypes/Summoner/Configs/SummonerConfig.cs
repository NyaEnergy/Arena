using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Archetypes/Summoner Config",
                 fileName = "SummonerConfig")]
public class SummonerConfig : ScriptableObject,
                              ICharacterConfig {
    [Header("Base")]
    [SerializeField] private float _maxHP = 95f;
    [SerializeField] private float _moveSpeed = 4.1f;
    [SerializeField, Min(0.1f)] private float _threatWeight = 3f;
    [SerializeField] private SummonerView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Ranged;

    [Header("Presence")]
    [SerializeField] private CharacterPresencePresentationConfig _entryPresentation;
    [SerializeField] private CharacterPresencePresentationConfig _exitPresentation;

    [Header("Summoning")]
    [SerializeField] private SummonedCharacterConfig _summonedCharacterConfig;
    [SerializeField] private Range _summonDistanceRange = new(4f, 8f);
    [SerializeField] private float _summonCooldown = 3f;
    [SerializeField] private int _maxSummons = 3;

    [Header("Spawn Position")]
    [SerializeField] private float _summonForwardOffset = 1.6f;
    [SerializeField] private float _summonSideOffset = 0.8f;
    [SerializeField] private float _navMeshSampleDistance = 1.5f;

    [Header("Movement")]
    [SerializeField] private float _retreatStepDistance = 2f;

    public CharacterType CharacterType => CharacterType.Summoner;
    public SummonedCharacterConfig SummonedCharacterConfig => _summonedCharacterConfig;
    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public float ThreatWeight => Mathf.Max(0.1f, _threatWeight);
    public CharacterView Prefab => _prefab;
    public CharacterCombatRow CombatRow => _combatRow;

    public CharacterPresencePresentationConfig EntryPresentation => _entryPresentation;
    public CharacterPresencePresentationConfig ExitPresentation => _exitPresentation;

    public Range SummonDistanceRange => _summonDistanceRange;
    public float SummonCooldown => _summonCooldown;
    public int MaxSummons => _maxSummons;

    public float SummonForwardOffset => _summonForwardOffset;
    public float SummonSideOffset => _summonSideOffset;
    public float NavMeshSampleDistance => _navMeshSampleDistance;

    public float RetreatStepDistance => _retreatStepDistance;
}