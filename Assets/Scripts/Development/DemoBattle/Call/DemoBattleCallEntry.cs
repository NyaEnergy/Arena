using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class DemoBattleCallEntry {
    [SerializeField] private Button _button;
    [SerializeField] private TeamType _teamType;
    [SerializeField] private DemoBattleCallType _callType;
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private SummonerConfig _summonerConfig;

    public Button Button => _button;
    public TeamType TeamType => _teamType;
    public DemoBattleCallType CallType => _callType;
    public CharacterType CharacterType => _characterType;
    public SummonerConfig SummonerConfig => _summonerConfig;

    public bool IsValid {
        get {
            if (_button == null) return false;

            if (_callType == DemoBattleCallType.Summoner) {
                return _summonerConfig != null;
            }

            return _characterType != CharacterType.Controller &&
                   _characterType != CharacterType.Summoner &&
                   _characterType != CharacterType.EliteCommander;
        }
    }
}