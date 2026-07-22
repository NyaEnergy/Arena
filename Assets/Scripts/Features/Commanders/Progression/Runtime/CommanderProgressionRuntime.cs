public sealed class CommanderProgressionRuntime {
    private readonly CommanderQuestProgress _questProgress;
    private readonly CommanderProgressionProgress _progressionProgress;

    public CommanderConfig Commander { get; }
    public CommanderProgressionNodeConfig Node { get; }

    public bool IsUnlocked => _progressionProgress.IsUnlocked(Commander, Node);
    public bool IsQuestCompleted => _questProgress.IsCompleted(Commander, Node.Quest);

    public bool ArePrerequisitesUnlocked {
        get {
            for (int i = 0; i < Node.Prerequisites.Count; ++i) {
                if (!_progressionProgress.IsUnlocked(
                        Commander, Node.Prerequisites[i])) return false;
            }

            return true;
        }
    }

    public bool CanUnlock => !IsUnlocked &&
                              IsQuestCompleted &&
                              ArePrerequisitesUnlocked;

    public CommanderProgressionRuntime(
                CommanderConfig commander,
                CommanderProgressionNodeConfig node,
                CommanderQuestProgress questProgress,
                CommanderProgressionProgress progressionProgress) {

        Commander = commander;
        Node = node;
        _questProgress = questProgress;
        _progressionProgress = progressionProgress;
    }

    internal bool TryUnlock() {
        return CanUnlock && _progressionProgress.Unlock(Commander, Node);
    }
}
