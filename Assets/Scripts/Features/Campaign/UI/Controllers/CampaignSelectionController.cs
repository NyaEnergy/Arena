using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public sealed class CampaignSelectionController : IInitializable,
                                                  IDisposable {
    private static readonly Color AvailableColor =
        new(0.2f, 0.85f, 0.95f, 1f);
    private static readonly Color SelectedColor =
        new(1f, 0.75f, 0.2f, 1f);
    private static readonly Color CompletedColor =
        new(0.2f, 0.9f, 0.5f, 1f);
    private static readonly Color LockedColor =
        new(0.55f, 0.6f, 0.65f, 1f);
    private static readonly Color ErrorColor =
        new(1f, 0.35f, 0.35f, 1f);

    private readonly CampaignConfig _config;
    private readonly CampaignProgress _progress;
    private readonly CampaignRuntime _runtime;
    private readonly CampaignService _service;
    private readonly CampaignSceneFlowService _sceneFlowService;
    private readonly CampaignSelectionView _view;
    private readonly List<AllyCommanderConfig> _alliedCommanders = new();
    private readonly List<EnemyCommanderConfig> _enemyCommanders = new();
    private readonly Dictionary<Button, UnityAction> _buttonActions = new();

    private AllyCommanderConfig _selectedAlliedCommander;
    private EnemyCommanderConfig _selectedEnemyCommander;

    public CampaignSelectionController(
        CampaignConfig config,
        CampaignProgress progress,
        CampaignRuntime runtime,
        CampaignService service,
        CampaignSceneFlowService sceneFlowService,
        CampaignSelectionView view) {

        _config = config ??
            throw new ArgumentNullException(nameof(config));
        _progress = progress ??
            throw new ArgumentNullException(nameof(progress));
        _runtime = runtime ??
            throw new ArgumentNullException(nameof(runtime));
        _service = service ??
            throw new ArgumentNullException(nameof(service));
        _sceneFlowService = sceneFlowService ??
            throw new ArgumentNullException(nameof(sceneFlowService));
        _view = view ??
            throw new ArgumentNullException(nameof(view));
    }

    public void Initialize() {
        BindButtons();
        _runtime.Changed += OnRuntimeChanged;

        SynchronizeSelection();
        Render();
    }

    public void Dispose() {
        _runtime.Changed -= OnRuntimeChanged;
        UnbindButtons();
    }

    private void BindButtons() {
        BindButton(_view.ConfirmButton,
                   ConfirmOrBeginTerritory);

        IReadOnlyList<CampaignArcOptionView> arcOptions =
            _view.ArcOptions;

        for (int i = 0; i < arcOptions.Count; ++i) {
            CampaignArcOptionView option = arcOptions[i];

            if (option == null) continue;

            BindButton(option.Button,
                       () => SelectArc(option));
        }

        BindCommanderButtons(_view.AlliedCommanderOptions);
        BindCommanderButtons(_view.EnemyCommanderOptions);
    }

    private void BindCommanderButtons(
        IReadOnlyList<CommanderSelectionOptionView> options) {

        for (int i = 0; i < options.Count; ++i) {
            CommanderSelectionOptionView option = options[i];

            if (option == null) continue;

            BindButton(option.Button,
                       () => SelectCommander(option));
        }
    }

    private void BindButton(Button button,
                            UnityAction action) {
        if (button == null ||
            action == null ||
            _buttonActions.ContainsKey(button)) return;

        button.onClick.AddListener(action);
        _buttonActions.Add(button, action);
    }

    private void UnbindButtons() {
        foreach (KeyValuePair<Button, UnityAction> pair
                 in _buttonActions) {
            if (pair.Key != null) {
                pair.Key.onClick.RemoveListener(pair.Value);
            }
        }

        _buttonActions.Clear();
    }

    private void SelectArc(CampaignArcOptionView option) {
        StoryArcConfig storyArc = option?.StoryArc;

        if (storyArc == null) return;

        _selectedAlliedCommander = null;
        _selectedEnemyCommander = null;

        if (_service.TryPrepare(storyArc)) return;

        _view.RenderStatus(
            "Эта арка пока недоступна.",
            ErrorColor);
    }

    private void SelectCommander(
        CommanderSelectionOptionView option) {

        if (_runtime.State !=
            CampaignState.CommanderSelection) return;

        CommanderConfig commander = option?.Commander;

        if (commander == null ||
            !_progress.IsOwned(commander)) return;

        if (commander is AllyCommanderConfig alliedCommander) {
            _selectedAlliedCommander = alliedCommander;
        } else if (commander is EnemyCommanderConfig enemyCommander) {
            _selectedEnemyCommander = enemyCommander;
        }

        Render();
    }

    private void ConfirmOrBeginTerritory() {
        if (_runtime.State ==
            CampaignState.CommanderSelection) {
            ConfirmSelection();
            return;
        }

        if (_runtime.State ==
            CampaignState.TerritoryReady) {
            BeginTerritory();
        }
    }

    private void ConfirmSelection() {
        if (!CanConfirmSelection()) return;

        if (_service.TrySelectCommanders(
                _selectedAlliedCommander,
                _selectedEnemyCommander)) return;

        _view.RenderStatus(
            "Не удалось закрепить выбранную пару.",
            ErrorColor);
    }

    private void BeginTerritory() {
        if (_sceneFlowService.TryEnterCurrentTerritory()) {
            return;
        }

        _view.RenderStatus(
            "Сцена территории недоступна. " +
            "Проверьте Scene Name и Build Settings.",
            ErrorColor);
    }

    private void OnRuntimeChanged() {
        SynchronizeSelection();
        Render();
    }

    private void SynchronizeSelection() {
        if (_runtime.State == CampaignState.TerritoryReady ||
            _runtime.State == CampaignState.TerritoryInProgress) {
            _selectedAlliedCommander =
                _runtime.AlliedCommander;
            _selectedEnemyCommander =
                _runtime.EnemyCommander;
            return;
        }

        if (_runtime.State !=
            CampaignState.CommanderSelection) {
            _selectedAlliedCommander = null;
            _selectedEnemyCommander = null;
            return;
        }

        if (!_progress.IsOwned(_selectedAlliedCommander)) {
            _selectedAlliedCommander = null;
        }

        if (!_progress.IsOwned(_selectedEnemyCommander)) {
            _selectedEnemyCommander = null;
        }
    }

    private void Render() {
        RenderArcOptions();
        BuildCommanderLists();

        bool isSelecting =
            _runtime.State == CampaignState.CommanderSelection;
        bool isFixed =
            _runtime.State == CampaignState.TerritoryReady ||
            _runtime.State == CampaignState.TerritoryInProgress;

        _view.SetCommanderSelectionVisible(
            isSelecting || isFixed);

        if (!isSelecting && !isFixed) {
            HideCommanderOptions();
            RenderWaitingForArc();
            return;
        }

        RenderCommanderOptions(
            _view.AlliedCommanderOptions,
            _alliedCommanders,
            _selectedAlliedCommander,
            isSelecting);

        RenderCommanderOptions(
            _view.EnemyCommanderOptions,
            _enemyCommanders,
            _selectedEnemyCommander,
            isSelecting);

        if (isFixed) {
            RenderFixedSelection();
            return;
        }

        RenderPendingSelection();
    }

    private void RenderArcOptions() {
        IReadOnlyList<CampaignArcOptionView> options =
            _view.ArcOptions;
        IReadOnlyList<StoryArcConfig> storyArcs =
            _config.StoryArcs;

        bool canChooseArc =
            _runtime.State == CampaignState.None ||
            _runtime.State == CampaignState.ArcCompleted;

        for (int i = 0; i < options.Count; ++i) {
            CampaignArcOptionView option = options[i];

            if (option == null) continue;

            if (i >= storyArcs.Count) {
                option.Hide();
                continue;
            }

            StoryArcConfig storyArc = storyArcs[i];
            bool isActive =
                !canChooseArc &&
                IsSameArc(_runtime.CurrentArc, storyArc);
            bool isCompleted =
                _progress.IsCompleted(storyArc);
            bool isAvailable =
                canChooseArc &&
                _service.CanPrepare(storyArc);

            ResolveArcStatus(isActive,
                             isCompleted,
                             isAvailable,
                             out string status,
                             out Color color);

            option.Render(storyArc,
                          status,
                          color,
                          isActive,
                          isAvailable);
        }
    }

    private void BuildCommanderLists() {
        _alliedCommanders.Clear();
        _enemyCommanders.Clear();

        IReadOnlyList<CommanderConfig> commanders =
            _progress.OwnedCommanders;

        for (int i = 0; i < commanders.Count; ++i) {
            CommanderConfig commander = commanders[i];

            if (commander is AllyCommanderConfig alliedCommander) {
                _alliedCommanders.Add(alliedCommander);
            } else if (commander is EnemyCommanderConfig enemyCommander) {
                _enemyCommanders.Add(enemyCommander);
            }
        }
    }

    private void RenderCommanderOptions<TCommander>(
        IReadOnlyList<CommanderSelectionOptionView> options,
        IReadOnlyList<TCommander> commanders,
        CommanderConfig selectedCommander,
        bool isInteractable)
        where TCommander : CommanderConfig {

        for (int i = 0; i < options.Count; ++i) {
            CommanderSelectionOptionView option = options[i];

            if (option == null) continue;

            if (i >= commanders.Count) {
                option.Hide();
                continue;
            }

            CommanderConfig commander = commanders[i];

            option.Render(
                commander,
                IsSameCommander(commander,
                                selectedCommander),
                isInteractable);
        }
    }

    private void HideCommanderOptions() {
        HideCommanderOptions(_view.AlliedCommanderOptions);
        HideCommanderOptions(_view.EnemyCommanderOptions);
    }

    private static void HideCommanderOptions(
        IReadOnlyList<CommanderSelectionOptionView> options) {

        for (int i = 0; i < options.Count; ++i) {
            options[i]?.Hide();
        }
    }

    private void RenderWaitingForArc() {
        _view.RenderConfirm("Подтвердить пару", false);

        string status =
            _runtime.State == CampaignState.ArcCompleted ?
            "Арка завершена. Выберите следующую." :
            "Выберите доступную сюжетную арку.";

        _view.RenderStatus(status, AvailableColor);
    }

    private void RenderPendingSelection() {
        bool hasAlliedCommander =
            _selectedAlliedCommander != null;
        bool hasEnemyCommander =
            _selectedEnemyCommander != null;

        _view.RenderConfirm(
            "Подтвердить пару",
            CanConfirmSelection());

        if (hasAlliedCommander && hasEnemyCommander) {
            _view.RenderStatus(
                "Пара готова. Подтвердите выбор.",
                SelectedColor);
        } else if (hasAlliedCommander) {
            _view.RenderStatus(
                "Теперь выберите вражеского командира.",
                AvailableColor);
        } else if (hasEnemyCommander) {
            _view.RenderStatus(
                "Теперь выберите союзного командира.",
                AvailableColor);
        } else {
            _view.RenderStatus(
                "Выберите по одному командиру с каждой стороны.",
                AvailableColor);
        }
    }

    private void RenderFixedSelection() {
        string alliedName =
            _selectedAlliedCommander?.DisplayName ??
            "—";
        string enemyName =
            _selectedEnemyCommander?.DisplayName ??
            "—";
        string territoryName =
            _runtime.CurrentTerritory?.DisplayName ??
            "—";

        if (_runtime.State ==
            CampaignState.TerritoryReady) {
            RenderReadyTerritory(
                alliedName,
                enemyName,
                territoryName);
            return;
        }

        _view.RenderConfirm(
            "Территория запущена",
            false);

        _view.RenderStatus(
            $"{territoryName}: {alliedName} против " +
            $"{enemyName}. Операция уже идёт.",
            CompletedColor);
    }

    private void RenderReadyTerritory(
        string alliedName,
        string enemyName,
        string territoryName) {

        bool canEnter =
            _sceneFlowService.CanEnterCurrentTerritory();

        _view.RenderConfirm(
            "Начать территорию",
            canEnter);

        if (!canEnter) {
            _view.RenderStatus(
                "Сцена территории недоступна. " +
                "Проверьте Scene Name и Build Settings.",
                ErrorColor);
            return;
        }

        _view.RenderStatus(
            $"{territoryName}: {alliedName} против " +
            $"{enemyName}. Пара закреплена до конца арки.",
            CompletedColor);
    }

    private bool CanConfirmSelection() {
        return _runtime.State ==
               CampaignState.CommanderSelection &&
               _selectedAlliedCommander != null &&
               _selectedEnemyCommander != null &&
               _progress.IsOwned(
                   _selectedAlliedCommander) &&
               _progress.IsOwned(
                   _selectedEnemyCommander);
    }

    private static void ResolveArcStatus(
        bool isActive,
        bool isCompleted,
        bool isAvailable,
        out string status,
        out Color color) {

        if (isActive) {
            status = "Выбрана";
            color = SelectedColor;
            return;
        }

        if (isCompleted) {
            status = "Пройдена";
            color = CompletedColor;
            return;
        }

        if (isAvailable) {
            status = "Доступна";
            color = AvailableColor;
            return;
        }

        status = "Закрыта";
        color = LockedColor;
    }

    private static bool IsSameArc(StoryArcConfig left,
                                  StoryArcConfig right) {
        return left != null &&
               right != null &&
               string.Equals(left.Id,
                             right.Id,
                             StringComparison.Ordinal);
    }

    private static bool IsSameCommander(CommanderConfig left,
                                        CommanderConfig right) {
        return left != null &&
               right != null &&
               string.Equals(left.Id,
                             right.Id,
                             StringComparison.Ordinal);
    }
}
