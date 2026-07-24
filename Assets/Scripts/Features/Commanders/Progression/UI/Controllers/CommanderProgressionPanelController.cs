using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public sealed class CommanderProgressionPanelController : IInitializable,
                                                          IDisposable {

    private static readonly Color UnlockedColor    = new(0.2f, 0.9f, 0.5f, 1f);
    private static readonly Color ProgressColor    = new(0.2f, 0.85f, 0.95f, 1f);
    private static readonly Color WaitingColor     = new(1f, 0.75f, 0.2f, 1f);
    private static readonly Color UnavailableColor = new(0.55f, 0.6f, 0.65f, 1f);

    private readonly CommanderProgressionService _progressionService;
    private readonly CommanderQuestService _questService;
    private readonly CommanderQuestProgress _questProgress;
    private readonly GameplaySceneSettings _settings;
    private readonly CommanderProgressionPanelView _view;

    private UnityAction _openAction;
    private UnityAction _closeAction;

    private readonly List<CommanderProgressionRuntime> _allyNodes = new();
    private readonly List<CommanderProgressionRuntime> _enemyNodes = new();

    public CommanderProgressionPanelController(
        CommanderProgressionService progressionService,
        CommanderQuestService questService,
        CommanderQuestProgress questProgress,
        GameplaySceneSettings settings,
        CommanderProgressionPanelView view) {

        _progressionService = progressionService ??
            throw new ArgumentNullException(nameof(progressionService));

        _questService = questService ??
            throw new ArgumentNullException(nameof(questService));

        _questProgress = questProgress ??
            throw new ArgumentNullException(nameof(questProgress));

        _settings = settings ??
            throw new ArgumentNullException(nameof(settings));

        _view = view ??
            throw new ArgumentNullException(nameof(view));
    }

    public void Initialize() {
        BuildNodeLists();

        _openAction = Open;
        _closeAction = Close;

        AddListener(_view.OpenButton, _openAction);
        AddListener(_view.CloseButton, _closeAction);

        _questService.ProgressChanged += OnQuestChanged;
        _questService.QuestCompleted += OnQuestChanged;
        _progressionService.NodeUnlocked += OnNodeUnlocked;

        _view.SetOpen(false);
        Render();
    }

    public void Dispose() {
        _questService.ProgressChanged -= OnQuestChanged;
        _questService.QuestCompleted -= OnQuestChanged;
        _progressionService.NodeUnlocked -= OnNodeUnlocked;

        RemoveListener(_view.OpenButton, _openAction);
        RemoveListener(_view.CloseButton, _closeAction);

        _openAction = null;
        _closeAction = null;
    }

    private void Open() {
        Render();
        _view.SetOpen(true);
    }

    private void Close() {
        _view.SetOpen(false);
    }

    private void OnQuestChanged(CommanderQuestRuntime quest) {
        Render();
    }

    private void OnNodeUnlocked(CommanderProgressionRuntime node) {
        Render();
    }

    private void BuildNodeLists() {
        _allyNodes.Clear();
        _enemyNodes.Clear();

        IReadOnlyList<CommanderProgressionRuntime> nodes =
            _progressionService.Nodes;

        for (int i = 0; i < nodes.Count; ++i) {
            CommanderProgressionRuntime node = nodes[i];

            if (node?.Commander == null) continue;

            if (node.Commander.TeamType == TeamType.Ally) {
                _allyNodes.Add(node);
            } else {
                _enemyNodes.Add(node);
            }
        }

        _allyNodes.Sort(CompareNodes);
        _enemyNodes.Sort(CompareNodes);
    }

    private void Render() {
        RenderTree(_view.AllyTree,
                   _settings.AlliedCommander,
                   _allyNodes);

        RenderTree(_view.EnemyTree,
                   _settings.EnemyCommander,
                   _enemyNodes);
    }

    private void RenderTree(
        CommanderProgressionTreeView treeView,
        CommanderConfig commander,
        IReadOnlyList<CommanderProgressionRuntime> nodes) {

        if (treeView == null) return;

        treeView.RenderHeader(commander);

        IReadOnlyList<CommanderProgressionNodeView> nodeViews =
            treeView.NodeViews;

        for (int i = 0; i < nodeViews.Count; ++i) {
            CommanderProgressionNodeView nodeView = nodeViews[i];

            if (nodeView == null) continue;

            if (i >= nodes.Count) {
                nodeView.Hide();
                continue;
            }

            RenderNode(nodeView, nodes[i]);
        }
    }

    private void RenderNode(CommanderProgressionNodeView view,
                            CommanderProgressionRuntime runtime) {

        CommanderProgressionNodeConfig node = runtime.Node;
        CommanderQuestConfig quest = node.Quest;

        float requiredAmount = quest.RequiredAmount;
        float currentAmount = Mathf.Clamp(
            _questProgress.GetValue(runtime.Commander, quest),
            0f,
            requiredAmount);

        ResolveStatus(runtime,
                      out string status,
                      out Color statusColor);

        string nodeType =
            node.NodeType == CommanderProgressionNodeType.Skill ?
            "Навык" :
            "Улучшение";

        view.Render(
            $"{nodeType}: {node.DisplayName}",
            node.Description,
            $"Квест: {quest.Title}\n{quest.Description}",
            currentAmount / requiredAmount,
            $"{FormatAmount(currentAmount)} / " +
            $"{FormatAmount(requiredAmount)}",
            status,
            statusColor,
            runtime.IsUnlocked);
    }

    private void ResolveStatus(CommanderProgressionRuntime runtime,
                               out string status,
                               out Color color) {

        if (runtime.IsUnlocked) {
            status = "Открыто";
            color = UnlockedColor;
            return;
        }

        if (runtime.IsQuestCompleted &&
            !runtime.ArePrerequisitesUnlocked) {
            status = "Ожидает предыдущий узел";
            color = WaitingColor;
            return;
        }

        if (runtime.IsQuestCompleted) {
            status = "Готово к открытию";
            color = ProgressColor;
            return;
        }

        if (!runtime.Node.Quest.IsAvailableOn(_settings.Territory)) {
            status = "Недоступно на этой территории";
            color = UnavailableColor;
            return;
        }

        status = "Квест выполняется";
        color = ProgressColor;
    }

    private static int CompareNodes(
        CommanderProgressionRuntime left,
        CommanderProgressionRuntime right) {

        int tierComparison =
            right.Node.Tier.CompareTo(left.Node.Tier);

        return tierComparison != 0 ?
            tierComparison :
            left.Node.Column.CompareTo(right.Node.Column);
    }

    private static string FormatAmount(float value) {
        return value.ToString("0.#");
    }

    private static void AddListener(Button button,
                                    UnityAction action) {
        if (button != null && action != null) {
            button.onClick.AddListener(action);
        }
    }

    private static void RemoveListener(Button button,
                                       UnityAction action) {
        if (button != null && action != null) {
            button.onClick.RemoveListener(action);
        }
    }
}