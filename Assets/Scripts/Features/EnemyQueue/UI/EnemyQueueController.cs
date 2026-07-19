using System;
using UnityEngine.Events;
using Zenject;

public class EnemyQueueController : IInitializable,
                                    IDisposable {
    private readonly EnemyQueueReleaseService _releaseService;
    private readonly EnemyQueueRuntime _runtime;
    private readonly EnemyQueueView _view;

    private UnityAction _releaseAction;

    public EnemyQueueController(EnemyQueueReleaseService releaseService,
                                EnemyQueueRuntime runtime,
                                EnemyQueueView view) {
        _releaseService = releaseService;
        _runtime = runtime;
        _view = view;
    }

    public void Initialize() {
        _runtime.Changed += Refresh;
        BindRelease();
        Refresh();
    }

    public void Dispose() {
        _runtime.Changed -= Refresh;

        if (_view != null &&
            _view.ReleaseButton != null &&
            _releaseAction != null) {
            _view.ReleaseButton.onClick.RemoveListener(_releaseAction);
        }

        _releaseAction = null;
    }

    private void BindRelease() {
        if (_view == null ||
            _view.ReleaseButton == null) {
            return;
        }

        _releaseAction = () => _releaseService.ReleaseNext();
        _view.ReleaseButton.onClick.AddListener(_releaseAction);
    }

    private void Refresh() {
        _view?.Render(_runtime);
    }
}