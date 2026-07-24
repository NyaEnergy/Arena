using System;
using UnityEngine;
using Zenject;

public sealed class StoryMissionController : IInitializable,
                                               IDisposable {
    private readonly StoryTaskService _service;
    private readonly StoryTaskRuntime _runtime;
    private readonly GameplaySceneSettings _settings;
    private readonly StoryMissionView _view;

    private StoryTaskPhase _renderedPhase;

    private int _dialogueIndex;
    private float _resumeTimeScale = 1f;
    private bool _hasRenderedPhase;
    private bool _isPaused;

    public StoryMissionController(
        StoryTaskService service,
        StoryTaskRuntime runtime,
        GameplaySceneSettings settings,
        StoryMissionView view) {

        _service = service ??
            throw new ArgumentNullException(nameof(service));
        _runtime = runtime ??
            throw new ArgumentNullException(nameof(runtime));
        _settings = settings ??
            throw new ArgumentNullException(nameof(settings));
        _view = view ??
            throw new ArgumentNullException(nameof(view));
    }

    public void Initialize() {
        _view.NextButton.onClick.AddListener(
            OnNextClicked);
        _view.SkipButton.onClick.AddListener(
            OnSkipClicked);
        _view.ResultButton.onClick.AddListener(
            OnResultClicked);
        _runtime.Changed += OnRuntimeChanged;

        Render();
    }

    public void Dispose() {
        _runtime.Changed -= OnRuntimeChanged;
        _view.NextButton.onClick.RemoveListener(
            OnNextClicked);
        _view.SkipButton.onClick.RemoveListener(
            OnSkipClicked);
        _view.ResultButton.onClick.RemoveListener(
            OnResultClicked);

        ResumeBattle();
    }

    private void OnRuntimeChanged() {
        Render();
    }

    private void OnNextClicked() {
        DialogueConfig dialogue =
            GetCurrentDialogue();

        if (dialogue == null ||
            !dialogue.IsValid) {
            _service.TryCompleteDialogue();
            return;
        }

        if (_dialogueIndex <
            dialogue.Lines.Count - 1) {
            ++_dialogueIndex;
            RenderDialogue(dialogue);
            return;
        }

        _service.TryCompleteDialogue();
    }

    private void OnSkipClicked() {
        _service.TryCompleteDialogue();
    }

    private void OnResultClicked() {
        StoryTaskPhase phase = _runtime.Phase;

        ResumeBattle();

        bool isSuccessful =
            phase == StoryTaskPhase.Victory ?
            _service.TryConfirmVictory() :
            phase == StoryTaskPhase.Defeat &&
            _service.TryRestart();

        if (isSuccessful) return;

        PauseBattle();
        _view.RenderResult(
            "ОШИБКА",
            "Не удалось изменить состояние территории.",
            phase == StoryTaskPhase.Victory ?
            "Продолжить" :
            "Повторить");
    }

    private void Render() {
        StoryTaskPhase phase = _runtime.Phase;
        bool isPhaseChanged =
            !_hasRenderedPhase ||
            _renderedPhase != phase;

        if (isPhaseChanged) {
            _renderedPhase = phase;
            _hasRenderedPhase = true;
            _dialogueIndex = 0;
        }

        switch (phase) {
            case StoryTaskPhase.Intro:
            case StoryTaskPhase.Outro:
                PauseBattle();
                RenderDialogue(
                    GetCurrentDialogue());
                break;

            case StoryTaskPhase.Active:
                ResumeBattle();
                RenderTask();
                break;

            case StoryTaskPhase.Victory:
                PauseBattle();
                RenderVictory();
                break;

            case StoryTaskPhase.Defeat:
                PauseBattle();
                RenderDefeat();
                break;

            default:
                ResumeBattle();
                _view.HideAll();
                break;
        }
    }

    private void RenderTask() {
        StoryTaskConfig task = _runtime.Task;

        if (task == null) {
            _view.HideAll();
            return;
        }

        float target = Mathf.Max(
            1f,
            task.TargetAmount);
        float progress = Mathf.Clamp(
            _runtime.Progress,
            0f,
            target);

        _view.RenderTask(
            task.Title,
            task.Description,
            CreateProgressText(task,
                               progress),
            progress / target);
    }

    private void RenderDialogue(
        DialogueConfig dialogue) {

        if (dialogue == null ||
            !dialogue.IsValid) {
            _service.TryCompleteDialogue();
            return;
        }

        _dialogueIndex = Mathf.Clamp(
            _dialogueIndex,
            0,
            dialogue.Lines.Count - 1);

        DialogueLine line =
            dialogue.Lines[_dialogueIndex];

        ResolveSpeaker(
            line,
            out string speakerName,
            out Sprite portrait);

        _view.RenderDialogue(
            speakerName,
            portrait,
            line.Text);
    }

    private void RenderVictory() {
        string territoryName =
            _settings.Territory.DisplayName;

        _view.RenderResult(
            "ПОБЕДА",
            $"{territoryName} пройдена.",
            "Продолжить");
    }

    private void RenderDefeat() {
        _view.RenderResult(
            "ПОРАЖЕНИЕ",
            "Союзный отряд потерян. " +
            "Территорию можно начать заново.",
            "Повторить");
    }

    private DialogueConfig GetCurrentDialogue() {
        StoryTaskConfig task = _runtime.Task;

        if (task == null) return null;

        return _runtime.Phase ==
               StoryTaskPhase.Intro ?
               task.IntroDialogue :
               _runtime.Phase ==
               StoryTaskPhase.Outro ?
               task.OutroDialogue :
               null;
    }

    private string CreateProgressText(
        StoryTaskConfig task,
        float progress) {

        int current =
            Mathf.FloorToInt(progress);

        return task.ObjectiveType ==
               StoryTaskObjectiveType.SurviveSeconds ?
               $"Продержаться: {current} / " +
               $"{task.TargetAmount} сек." :
               $"Устранено: {current} / " +
               $"{task.TargetAmount}";
    }

    private void ResolveSpeaker(
        DialogueLine line,
        out string speakerName,
        out Sprite portrait) {

        CommanderConfig commander =
            GetSpeakerCommander(line.SpeakerType);

        speakerName =
            string.IsNullOrWhiteSpace(
                line.SpeakerNameOverride) ?
            commander?.DisplayName ?? "—" :
            line.SpeakerNameOverride;

        portrait =
            line.PortraitOverride != null ?
            line.PortraitOverride :
            commander?.Icon;
    }

    private CommanderConfig GetSpeakerCommander(
        DialogueSpeakerType speakerType) {

        if (speakerType ==
            DialogueSpeakerType.AlliedCommander) {
            return _settings.AlliedCommander;
        }

        if (speakerType ==
            DialogueSpeakerType.EnemyCommander) {
            return _settings.EnemyCommander;
        }

        return null;
    }

    private void PauseBattle() {
        if (_isPaused) return;

        _resumeTimeScale =
            Time.timeScale > 0f ?
            Time.timeScale :
            1f;

        Time.timeScale = 0f;
        _isPaused = true;
    }

    private void ResumeBattle() {
        if (!_isPaused) return;

        Time.timeScale = _resumeTimeScale;
        _isPaused = false;
    }
}
