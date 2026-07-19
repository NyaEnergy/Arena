using UnityEngine;

[System.Serializable]
public class EnemyConveyorEntry {
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private SummonerConfig _summonerConfig;
    [SerializeField] private Sprite _icon;

    public EnemyQueueItem CreateItem() {
        EnemyQueueItem item = new(
            _characterType,
            _summonerConfig,
            _icon);

        return item.IsValid ? item : null;
    }
}
