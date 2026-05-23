using UnityEngine;

public class MovementComponent {
    private readonly CharacterView _view;
    private readonly CharacterConfig _config;

    public MovementComponent(CharacterView view, CharacterConfig config) {
        _view = view;
        _config = config;

        Initialize();
    }

    public void MoveTo(Vector3 position) {
        if (!CanUseAgent()) return;
        _view.Agent.isStopped = false;
        _view.Agent.SetDestination(position);
    }

    public void Stop() {
        if (!CanUseAgent()) return;
        _view.Agent.isStopped = true;
    }

    private void Initialize() {
        if (!CanUseAgent()) return;
        _view.Agent.speed = _config.MoveSpeed;
    }

    private bool CanUseAgent() {
        if(_view.Agent == null) return false;
        if(_view.Agent.enabled == false) return false;
        if(_view.Agent.isOnNavMesh == false) return false;
        return true;
    }
}
