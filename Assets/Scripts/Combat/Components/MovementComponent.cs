using UnityEngine;

public class MovementComponent {
    private readonly CharacterView _view;
    private readonly CharacterConfig _config;

    private bool IsCanUseAgent =>
        _view.Agent != null &&
        _view.Agent.enabled &&
        _view.Agent.isOnNavMesh;

    public MovementComponent(CharacterView view,
                             CharacterConfig config) {
        _view = view;
        _config = config;

        Reset();
    }

    public void Reset() {
        if (_view.Agent == null) return;
        _view.Agent.speed = _config.MoveSpeed;
        if (!IsCanUseAgent) return;
        _view.Agent.ResetPath();
        _view.Agent.isStopped = true;
    }

    public void MoveTo(Vector3 position) {
        if (!IsCanUseAgent) return;
        _view.Agent.isStopped = false;
        _view.Agent.SetDestination(position);
    }

    public void Stop() {
        if (!IsCanUseAgent) return;
        _view.Agent.isStopped = true;
    }
}
