using System;
using System.Collections.Generic;
using UnityEngine;

public sealed class CommanderQuestService {
    private readonly List<CommanderQuestRuntime> _activeQuests = new();

    public event Action<CommanderQuestRuntime> ProgressChanged;
    public event Action<CommanderQuestRuntime> QuestCompleted;

    public IReadOnlyList<CommanderQuestRuntime> ActiveQuests => _activeQuests;

    public CommanderQuestService(GameplaySceneSettings settings,
                                 CommanderQuestProgress progress) {
        if (settings == null ||
            !settings.IsValid) {
            throw new ArgumentException(
                "Commander quests require valid gameplay scene settings.",
                nameof(settings));
        }

        if (progress == null) {
            throw new ArgumentNullException(nameof(progress));
        }

        AddCommander(settings.AlliedCommander,
                     settings.Territory,
                     progress);

        AddCommander(settings.EnemyCommander,
                     settings.Territory,
                     progress);
    }

    public void Report(CommanderQuestEvent questEvent) {
        if (!questEvent.IsValid) return;

        for (int i = 0; i < _activeQuests.Count; ++i) {
            CommanderQuestRuntime runtime = _activeQuests[i];

            if (runtime.IsCompleted ||
                !runtime.Quest.Matches(
                    questEvent,
                    runtime.Commander.TeamType)) continue;

            if (!runtime.TryAdd(questEvent.Amount,
                                out bool justCompleted)) continue;

            ProgressChanged?.Invoke(runtime);

            if (justCompleted) {
                Debug.Log($"[CommanderQuest] Completed: " +
                          $"{runtime.Commander.DisplayName} / " +
                          $"{runtime.Quest.Title}");

                QuestCompleted?.Invoke(runtime);
            }
        }
    }

    private void AddCommander(CommanderConfig commander,
                              StoryTerritoryConfig territory,
                              CommanderQuestProgress progress) {

        if (commander == null || !commander.IsValid) return;

        for (int i = 0; i < commander.Quests.Count; ++i) {
            CommanderQuestConfig quest = commander.Quests[i];

            if (!quest.IsAvailableOn(territory) ||
                progress.IsCompleted(commander, quest)) continue;

            _activeQuests.Add(new CommanderQuestRuntime(
                commander, quest, progress));
        }
    }
}
