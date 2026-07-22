using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Campaign/Story Arc Config",
                 fileName = "StoryArcConfig")]
public sealed class StoryArcConfig : ScriptableObject {
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;

    [Header("Granted Commanders")]
    [SerializeField] private List<AllyCommanderConfig> _grantedAlliedCommanders = new();
    [SerializeField] private List<EnemyCommanderConfig> _grantedEnemyCommanders = new();

    [Header("Ordered Territories")]
    [SerializeField] private List<StoryTerritoryConfig> _territories = new();

    public string Id =>
        string.IsNullOrWhiteSpace(_id) ?
        string.Empty : _id.Trim();

    public string DisplayName =>
        string.IsNullOrWhiteSpace(_displayName) ?
        name : _displayName.Trim();

    public IReadOnlyList<AllyCommanderConfig> GrantedAlliedCommanders => _grantedAlliedCommanders;
    public IReadOnlyList<EnemyCommanderConfig> GrantedEnemyCommanders => _grantedEnemyCommanders;
    public IReadOnlyList<StoryTerritoryConfig> Territories => _territories;

    public bool IsValid {
        get {
            if (string.IsNullOrWhiteSpace(Id) ||
                _grantedAlliedCommanders == null ||
                _grantedAlliedCommanders.Count == 0 ||
                _grantedEnemyCommanders == null ||
                _grantedEnemyCommanders.Count == 0 ||
                _territories == null ||
                _territories.Count == 0) return false;

            for (int i = 0; i < _grantedAlliedCommanders.Count; ++i) {
                if (_grantedAlliedCommanders[i] == null ||
                    !_grantedAlliedCommanders[i].IsValid) return false;
            }

            for (int i = 0; i < _grantedEnemyCommanders.Count; ++i) {
                if (_grantedEnemyCommanders[i] == null ||
                    !_grantedEnemyCommanders[i].IsValid) return false;
            }

            for (int i = 0; i < _territories.Count; ++i) {
                if (_territories[i] == null ||
                    !_territories[i].IsValid) return false;
            }

            return true;
        }
    }
}
