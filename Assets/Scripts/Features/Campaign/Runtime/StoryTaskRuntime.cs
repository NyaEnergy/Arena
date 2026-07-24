using System;
using UnityEngine;

public sealed class StoryTaskRuntime {
    private StoryTaskConfig _task;
    private StoryTaskPhase _phase;

    private float _progress;
    private bool _isLastTask;

    public event Action Changed;

    public StoryTaskConfig Task => _task;
    public StoryTaskPhase Phase => _phase;
    public float Progress => _progress;
    public int TargetAmount => _task?.TargetAmount ?? 0;
    public bool IsLastTask => _isLastTask;

    public bool IsCurrent(string storyTaskId) {
        return _task != null &&
               !string.IsNullOrWhiteSpace(storyTaskId) &&
               string.Equals(
                   _task.Id,
                   storyTaskId.Trim(),
                   StringComparison.Ordinal);
    }

    internal void Prepare(StoryTaskConfig task,
                          bool isLastTask) {
        _task = task;
        _isLastTask = isLastTask;
        _progress = 0f;
        _phase = StoryTaskPhase.Intro;
        NotifyChanged();
    }

    internal bool TryBegin() {
        if (_phase != StoryTaskPhase.Intro ||
            _task == null) return false;

        _phase = StoryTaskPhase.Active;
        NotifyChanged();
        return true;
    }

    internal void AddProgress(float amount) {
        if (_phase != StoryTaskPhase.Active ||
            _task == null ||
            amount <= 0f) return;

        _progress = Mathf.Min(
            _progress + amount,
            _task.TargetAmount);

        if (_progress >= _task.TargetAmount) {
            _phase = StoryTaskPhase.Outro;
        }

        NotifyChanged();
    }

    internal bool TryShowVictory() {
        if (_phase != StoryTaskPhase.Outro ||
            !_isLastTask) return false;

        _phase = StoryTaskPhase.Victory;
        NotifyChanged();
        return true;
    }

    internal void Fail() {
        if (_phase != StoryTaskPhase.Active) return;

        _phase = StoryTaskPhase.Defeat;
        NotifyChanged();
    }

    internal void Reset() {
        if (_task == null &&
            _phase == StoryTaskPhase.None &&
            _progress <= 0f &&
            !_isLastTask) return;

        _task = null;
        _phase = StoryTaskPhase.None;
        _progress = 0f;
        _isLastTask = false;
        NotifyChanged();
    }

    private void NotifyChanged() {
        Changed?.Invoke();
    }
}
