using System;

public readonly struct CommanderProgressionKey :
                       IEquatable<CommanderProgressionKey> {
    public string CommanderId { get; }
    public string NodeId { get; }

    public CommanderProgressionKey(string commanderId,
                                   string nodeId) {
        CommanderId = commanderId ?? string.Empty;
        NodeId = nodeId ?? string.Empty;
    }

    public bool Equals(CommanderProgressionKey other) {
        return string.Equals(CommanderId,
                             other.CommanderId,
                             StringComparison.Ordinal) &&
               string.Equals(NodeId,
                             other.NodeId,
                             StringComparison.Ordinal);
    }

    public override bool Equals(object obj) {
        return obj is CommanderProgressionKey other &&
               Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(CommanderId, NodeId);
    }
}
