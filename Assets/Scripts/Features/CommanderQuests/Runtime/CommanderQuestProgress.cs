using System.Collections.Generic;
using UnityEngine;

public sealed class CommanderQuestProgress {
    private readonly Dictionary<CommanderQuestKey, float> _values = new();
    private readonly HashSet<CommanderQuestKey> _completed = new();

    public float GetValue(CommanderConfig commander,
                          CommanderQuestConfig quest) {

        if (!TryCreateKey(commander, quest,
                          out CommanderQuestKey key)) return 0f;

        return _values.TryGetValue(key, out float value) ?
               value : 0f;
    }

    public bool IsCompleted(CommanderConfig commander,
                            CommanderQuestConfig quest) {
        return TryCreateKey(commander, quest,
                            out CommanderQuestKey key) &&
               _completed.Contains(key);
    }

    internal bool TryAdd(CommanderConfig commander,
                         CommanderQuestConfig quest,
                         float amount) {
        if (amount <= 0f ||
            float.IsNaN(amount) ||
            float.IsInfinity(amount) ||
            !TryCreateKey(commander, quest,
                          out CommanderQuestKey key) ||
            _completed.Contains(key)) return false;

        float current = _values.TryGetValue(key, out float value) ?
                        value : 0f;

        float next = Mathf.Min(quest.RequiredAmount,
                               current + amount);

        if (next <= current) return false;

        _values[key] = next;

        if (next >= quest.RequiredAmount) {
            _completed.Add(key);
        }

        return true;
    }

    private static bool TryCreateKey(CommanderConfig commander,
                                     CommanderQuestConfig quest,
                                     out CommanderQuestKey key) {
        key = default;

        if (commander == null ||
            quest == null ||
            string.IsNullOrWhiteSpace(commander.Id) ||
            string.IsNullOrWhiteSpace(quest.Id)) return false;

        key = new CommanderQuestKey(commander.Id, quest.Id);

        return true;
    }
}
