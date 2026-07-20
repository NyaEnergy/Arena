using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Commander/Enemy Group Config",
                 fileName = "EnemyGroupConfig")]
public sealed class EnemyGroupConfig : ScriptableObject {
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private SummonerConfig _summonerConfig;
    [SerializeField] private Sprite _icon;

    [Header("Group")]
    [SerializeField, Min(1)] private int _count = 3;
    [SerializeField, Min(0.25f)] private float _formationSpacing = 1.25f;

    public CharacterType CharacterType => _characterType;
    public SummonerConfig SummonerConfig => _summonerConfig;
    public Sprite Icon => _icon;
    public int Count => Mathf.Max(1, _count);
    public float FormationSpacing => Mathf.Max(0.25f, _formationSpacing);

    public bool IsValid {
        get {
            if (_icon == null ||
                _characterType == CharacterType.Medic ||
                _characterType == CharacterType.EliteCommander) {
                return false;
            }

            if (_characterType == CharacterType.Summoner) {
                return _summonerConfig != null;
            }

            return _summonerConfig == null;
        }
    }

    public EnemyQueueItem CreateItem() {
        return IsValid ? new EnemyQueueItem(this) : null;
    }
}
