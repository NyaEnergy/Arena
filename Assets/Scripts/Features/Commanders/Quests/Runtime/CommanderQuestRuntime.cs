using UnityEngine;

public sealed class CommanderQuestRuntime {
    private readonly CommanderQuestProgress _progress;

    public CommanderConfig Commander { get; }
    public CommanderQuestConfig Quest { get; }

    public float CurrentAmount =>
        _progress.GetValue(Commander, Quest);

    public float NormalizedProgress => Quest == null ?
        0f : Mathf.Clamp01(CurrentAmount / Quest.RequiredAmount);

    public bool IsCompleted => _progress.IsCompleted(Commander, Quest);

    public CommanderQuestRuntime(CommanderConfig commander,
                                 CommanderQuestConfig quest,
                                 CommanderQuestProgress progress) {
        Commander = commander;
        Quest = quest;
        _progress = progress;
    }

    internal bool TryAdd(float amount,
                     out bool justCompleted) {

        bool wasCompleted = IsCompleted;
        bool changed = _progress.TryAdd(Commander, Quest, amount);

        justCompleted = changed &&
                       !wasCompleted &&
                        IsCompleted;

        return changed;
    }
}
