using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class CommanderConfig : ScriptableObject {
    [Header("Identity")]
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private Sprite _icon;

    [Header("Quests")]
    [SerializeField] private List<CommanderQuestConfig> _quests = new();

    [Header("Progression")]
    [SerializeField] private CommanderProgressionTreeConfig _progressionTree;

    public string Id => string.IsNullOrWhiteSpace(_id) ?
                        string.Empty : _id.Trim();

    public string DisplayName =>
        string.IsNullOrWhiteSpace(_displayName) ?
        name : _displayName.Trim();

    public Sprite Icon => _icon;
    public IReadOnlyList<CommanderQuestConfig> Quests => _quests;
    public CommanderProgressionTreeConfig ProgressionTree => _progressionTree;
    public abstract TeamType TeamType { get; }

    public bool IsValid {
        get {
            if (string.IsNullOrWhiteSpace(Id) ||
                _quests == null ||
                _quests.Count == 0) return false;

            HashSet<string> questIds = new(StringComparer.Ordinal);

            for (int i = 0; i < _quests.Count; ++i) {
                CommanderQuestConfig quest = _quests[i];

                if (quest == null ||
                    !quest.IsValid ||
                    !questIds.Add(quest.Id)) return false;
            }

            return _progressionTree != null &&
                   _progressionTree.IsValid &&
                   _progressionTree.UsesExactly(_quests);
        }
    }
}
