using System.Collections.Generic;

public sealed class CommanderProgressionProgress {
    private readonly HashSet<CommanderProgressionKey> _unlocked = new();

    public bool IsUnlocked(CommanderConfig commander,
                           CommanderProgressionNodeConfig node) {

        return TryCreateKey(commander, node, out CommanderProgressionKey key) &&
               _unlocked.Contains(key);
    }

    internal bool Unlock(CommanderConfig commander,
                         CommanderProgressionNodeConfig node) {

        return TryCreateKey(commander, node, out CommanderProgressionKey key) &&
               _unlocked.Add(key);
    }

    private static bool TryCreateKey(CommanderConfig commander,
                                     CommanderProgressionNodeConfig node,
                                 out CommanderProgressionKey key) {
        key = default;

        if (commander == null ||
            node == null ||
            string.IsNullOrWhiteSpace(commander.Id) ||
            string.IsNullOrWhiteSpace(node.Id)) return false;

        key = new CommanderProgressionKey(commander.Id, node.Id);

        return true;
    }
}
