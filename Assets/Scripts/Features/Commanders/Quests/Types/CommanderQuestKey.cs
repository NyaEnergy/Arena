using System;

public readonly struct CommanderQuestKey : IEquatable<CommanderQuestKey> {
    public string CommanderId { get; }
    public string QuestId { get; }

    public CommanderQuestKey(string commanderId, string questId) {

        CommanderId = commanderId ?? string.Empty;
        QuestId = questId ?? string.Empty;
    }

    public bool Equals(CommanderQuestKey other) {
        return string.Equals(CommanderId,
                             other.CommanderId,
                             StringComparison.Ordinal) &&
               string.Equals(QuestId,
                             other.QuestId,
                             StringComparison.Ordinal);
    }

    public override bool Equals(object obj) {
        return obj is CommanderQuestKey other &&
               Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(CommanderId, QuestId);
    }
}
