using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Archetypes/Controller Config",
                 fileName = "ControllerConfig")]
public class ControllerConfig : ScriptableObject,
                                ICharacterConfig {

    [Header("Base")]
    [SerializeField] private float _maxHP = 105f;
    [SerializeField] private float _moveSpeed = 4.2f;
    [SerializeField, Min(0.1f)] private float _threatWeight = 2f;
    [SerializeField] private ControllerView _prefab;
    [SerializeField] private CharacterCombatRow _combatRow = CharacterCombatRow.Support;

    [Header("Presence")]
    [SerializeField] private CharacterPresencePresentationConfig _entryPresentation;
    [SerializeField] private CharacterPresencePresentationConfig _exitPresentation;

    [Header("Positioning")]
    [SerializeField] private Range _controlDistanceRange = new(4f, 7f);
    [SerializeField] private float _retreatStepDistance = 2f;

    [Header("Field")]
    [SerializeField] private ControllerFieldView _fieldPrefab;
    [SerializeField] private float _fieldRadius = 3f;
    [SerializeField] private float _fieldDuration = 4f;
    [SerializeField] private float _fieldCooldown = 6f;

    [SerializeField, Range(0f, 1f)] private float _slowMultiplier = 0.5f;

    [SerializeField] private Color _allyFieldColor = new(0.15f, 0.9f, 1f, 0.3f);
    [SerializeField] private Color _enemyFieldColor = new(1f, 0.15f, 0.3f, 0.3f);

    public CharacterType CharacterType => CharacterType.Controller;

    public float MaxHP => _maxHP;
    public float MoveSpeed => _moveSpeed;
    public float ThreatWeight => Mathf.Max(0.1f, _threatWeight);
    public CharacterView Prefab => _prefab;
    public CharacterCombatRow CombatRow => _combatRow;
    public CharacterPresencePresentationConfig EntryPresentation => _entryPresentation;
    public CharacterPresencePresentationConfig ExitPresentation => _exitPresentation;
    public Range ControlDistanceRange => _controlDistanceRange;
    public float RetreatStepDistance => _retreatStepDistance;
    public ControllerFieldView FieldPrefab => _fieldPrefab;
    public float FieldRadius => _fieldRadius;
    public float FieldDuration => _fieldDuration;
    public float FieldCooldown => _fieldCooldown;

    public float SlowMultiplier => _slowMultiplier;

    public Color GetFieldColor(TeamType teamType) {
        return teamType == TeamType.Ally ?
                           _allyFieldColor : _enemyFieldColor;
    }
}