using UnityEngine;

public class MovementComponent : MonoBehaviour {
    private readonly CharacterView _view;
    private readonly CharacterConfig _config;

    public MovementComponent(CharacterView view, CharacterConfig config) {
        _view = view;
        _config = config;

        Initialize();
    }

    public void MoveTo(Vector3 position) {
        _view.Agent.isStopped = false;
        _view.Agent.SetDestination(position);
    }

    public void Stop() {
        _view.Agent.isStopped = true;
    }

    private void Initialize() {
        _view.Agent.speed = _config.MoveSpeed;
    }
}
