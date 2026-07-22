using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class CommanderProgressionService : IInitializable,
                                                  IDisposable {

    private readonly CommanderQuestService _questService;
    private readonly List<CommanderProgressionRuntime> _nodes = new();

    public event Action<CommanderProgressionRuntime> NodeUnlocked;

    public IReadOnlyList<CommanderProgressionRuntime> Nodes => _nodes;

    public CommanderProgressionService(
                GameplaySceneSettings settings,
                CommanderQuestService questService,
                CommanderQuestProgress questProgress,
                CommanderProgressionProgress progressionProgress) {

        if (settings == null ||
            !settings.IsValid) {
            throw new ArgumentException(
                "Commander progression requires valid gameplay settings.",
                nameof(settings));
        }

        _questService = questService ??
                        throw new ArgumentNullException(nameof(questService));

        if (questProgress == null) {
            throw new ArgumentNullException(nameof(questProgress));
        }

        if (progressionProgress == null) {
            throw new ArgumentNullException(nameof(progressionProgress));
        }

        AddCommander(settings.AlliedCommander,
                     questProgress,
                     progressionProgress);

        AddCommander(settings.EnemyCommander,
                     questProgress,
                     progressionProgress);
    }

    public void Initialize() {
        _questService.QuestCompleted += OnQuestCompleted;
        RefreshUnlocks();
    }

    public void Dispose() {
        _questService.QuestCompleted -= OnQuestCompleted;
    }

    private void OnQuestCompleted(CommanderQuestRuntime quest) {
        RefreshUnlocks();
    }

    private void RefreshUnlocks() {
        bool unlocked;

        do {
            unlocked = false;

            for (int i = 0; i < _nodes.Count; ++i) {
                CommanderProgressionRuntime runtime = _nodes[i];

                if (!runtime.TryUnlock()) continue;

                unlocked = true;

                Debug.Log($"[CommanderProgression] Unlocked: " +
                          $"{runtime.Commander.DisplayName} / " +
                          $"{runtime.Node.DisplayName}");

                NodeUnlocked?.Invoke(runtime);
            }
        } while (unlocked);
    }

    private void AddCommander(
        CommanderConfig commander,
        CommanderQuestProgress questProgress,
        CommanderProgressionProgress progressionProgress) {
        if (commander == null ||
            !commander.IsValid) return;

        IReadOnlyList<CommanderProgressionNodeConfig> nodes =
            commander.ProgressionTree.Nodes;

        for (int i = 0; i < nodes.Count; ++i) {
            _nodes.Add(new CommanderProgressionRuntime(
                commander,
                nodes[i],
                questProgress,
                progressionProgress));
        }
    }
}
