using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Commanders/Progressions/Quest Config",
                 fileName = "CommanderQuestConfig")]
public sealed class CommanderQuestConfig : ScriptableObject {
    [Header("Identity")]
    [SerializeField] private string _id;
    [SerializeField] private string _title;
    [SerializeField, TextArea] private string _description;

    [Header("Objective")]
    [SerializeField] private CommanderQuestEventType _eventType;

    [SerializeField] private CommanderQuestTeamRelation _teamRelation =
                             CommanderQuestTeamRelation.Commander;

    [SerializeField] private bool _filterByCharacterType;
    [SerializeField] private CharacterType _characterType;
    [SerializeField, Min(1f)] private float _requiredAmount = 1f;

    [Header("Territory Availability")]
    [SerializeField] private bool _availableOnAnyTerritory = true;
    [SerializeField] private List<StoryTerritoryConfig> _territories = new();

    public string Id => string.IsNullOrWhiteSpace(_id) ?
                        string.Empty : _id.Trim();

    public string Title => string.IsNullOrWhiteSpace(_title) ?
                           name : _title.Trim();

    public string Description => _description;
    public CommanderQuestEventType EventType => _eventType;
    public CommanderQuestTeamRelation TeamRelation => _teamRelation;
    public bool FilterByCharacterType => _filterByCharacterType;
    public CharacterType CharacterType => _characterType;
    public float RequiredAmount => Mathf.Max(1f, _requiredAmount);
    public bool AvailableOnAnyTerritory => _availableOnAnyTerritory;
    public IReadOnlyList<StoryTerritoryConfig> Territories => _territories;

    public bool IsValid {
        get {
            if (string.IsNullOrWhiteSpace(Id) ||
                _requiredAmount <= 0f ||
                float.IsNaN(_requiredAmount) ||
                float.IsInfinity(_requiredAmount)) return false;

            if (_availableOnAnyTerritory) return true;

            if (_territories == null ||
                _territories.Count == 0) return false;

            HashSet<string> territoryIds = new(StringComparer.Ordinal);

            for (int i = 0; i < _territories.Count; ++i) {
                StoryTerritoryConfig territory = _territories[i];

                if (territory == null ||
                    !territory.IsValid ||
                    !territoryIds.Add(territory.Id)) return false;
            }

            return true;
        }
    }

    public bool IsAvailableOn(StoryTerritoryConfig territory) {
        if (!IsValid ||
            territory == null ||
            !territory.IsValid) return false;

        if (_availableOnAnyTerritory) return true;

        for (int i = 0; i < _territories.Count; ++i) {
            if (string.Equals(_territories[i].Id,
                              territory.Id,
                              StringComparison.Ordinal)) return true;
        }

        return false;
    }

    public bool Matches(CommanderQuestEvent questEvent,
                        TeamType commanderTeam) {
        if (!questEvent.IsValid ||
            questEvent.EventType != _eventType ||
            !MatchesTeam(questEvent.TeamType, commanderTeam)) return false;

        return !_filterByCharacterType ||
               questEvent.CharacterType == _characterType;
    }

    private bool MatchesTeam(TeamType eventTeam,
                             TeamType commanderTeam) {
        return _teamRelation switch {
            CommanderQuestTeamRelation.Any => true,
            CommanderQuestTeamRelation.Commander =>
                eventTeam == commanderTeam,
            CommanderQuestTeamRelation.Opponent =>
                eventTeam != commanderTeam,
            _ => false
        };
    }
}
