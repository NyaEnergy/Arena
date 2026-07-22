using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Commanders/Progressions/Tree Config",
                 fileName = "CommanderProgressionTreeConfig")]
public sealed class CommanderProgressionTreeConfig : ScriptableObject {
    [SerializeField] private string _id;
    [SerializeField] private string _displayName;
    [SerializeField] private List<CommanderProgressionNodeConfig> _nodes = new();

    public string Id => string.IsNullOrWhiteSpace(_id) ?
                        string.Empty : _id.Trim();

    public string DisplayName =>
        string.IsNullOrWhiteSpace(_displayName) ?
        name : _displayName.Trim();

    public IReadOnlyList<CommanderProgressionNodeConfig> Nodes => _nodes;

    public bool IsValid {
        get {
            if (string.IsNullOrWhiteSpace(Id) ||
                _nodes == null ||
                _nodes.Count == 0) return false;

            HashSet<CommanderProgressionNodeConfig> nodeSet = new();
            HashSet<string> nodeIds = new(StringComparer.Ordinal);
            HashSet<string> questIds = new(StringComparer.Ordinal);
            HashSet<Vector2Int> positions = new();

            for (int i = 0; i < _nodes.Count; ++i) {
                CommanderProgressionNodeConfig node = _nodes[i];

                if (node == null ||
                    !node.IsValid ||
                    !nodeSet.Add(node) ||
                    !nodeIds.Add(node.Id) ||
                    !questIds.Add(node.Quest.Id) ||
                    !positions.Add(new Vector2Int(
                        node.Column, node.Tier))) return false;
            }

            for (int i = 0; i < _nodes.Count; ++i) {
                CommanderProgressionNodeConfig node = _nodes[i];
                bool isRoot = node.Prerequisites.Count == 0;

                if ((node.Tier == 0) != isRoot) return false;

                for (int prerequisiteIndex = 0;
                     prerequisiteIndex < node.Prerequisites.Count;
                     ++prerequisiteIndex) {
                    CommanderProgressionNodeConfig prerequisite =
                        node.Prerequisites[prerequisiteIndex];

                    if (!nodeSet.Contains(prerequisite) ||
                        prerequisite.Tier >= node.Tier) return false;
                }
            }

            return !HasCycle();
        }
    }

    public bool UsesExactly(
        IReadOnlyList<CommanderQuestConfig> quests) {
        if (!IsValid ||
            quests == null ||
            quests.Count != _nodes.Count) return false;

        HashSet<CommanderQuestConfig> questSet = new();

        for (int i = 0; i < quests.Count; ++i) {
            CommanderQuestConfig quest = quests[i];

            if (quest == null ||
                !quest.IsValid ||
                !questSet.Add(quest)) return false;
        }

        for (int i = 0; i < _nodes.Count; ++i) {
            if (!questSet.Remove(_nodes[i].Quest)) return false;
        }

        return questSet.Count == 0;
    }

    private bool HasCycle() {
        Dictionary<CommanderProgressionNodeConfig, int> states = new();

        for (int i = 0; i < _nodes.Count; ++i) {
            if (Visit(_nodes[i], states)) return true;
        }

        return false;
    }

    private static bool Visit(
        CommanderProgressionNodeConfig node,
        Dictionary<CommanderProgressionNodeConfig, int> states) {
        if (states.TryGetValue(node, out int state)) {
            return state == 1;
        }

        states[node] = 1;

        for (int i = 0; i < node.Prerequisites.Count; ++i) {
            if (Visit(node.Prerequisites[i], states)) return true;
        }

        states[node] = 2;
        return false;
    }
}
