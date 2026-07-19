using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class EnemyQueueCardEntry {
    [SerializeField] private Button _button;
    [SerializeField] private Image _iconImage;
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private SummonerConfig _summonerConfig;

    public Button Button => _button;

    public bool IsValid {
        get {
            if (_button == null ||
                _iconImage == null ||
                _iconImage.sprite == null) {
                return false;
            }

            if (_characterType == CharacterType.Summoner) {
                return _summonerConfig != null;
            }

            return _characterType != CharacterType.Summoner &&
                   _characterType != CharacterType.EliteCommander;
        }
    }

    public EnemyQueueItem CreateItem() {
        if (!IsValid) return null;

        return new EnemyQueueItem(_characterType,
                                  _summonerConfig,
                                  _iconImage.sprite);
    }
}