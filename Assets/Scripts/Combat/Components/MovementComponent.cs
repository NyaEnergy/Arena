using UnityEngine;

public class MovementComponent {
    private const float ATTACK_DISTANCE_BUFFER = 0.2f;
    private const float POSITION_STOPPING_DISTANCE = 0.1f;

    private readonly CharacterView _view;
    private readonly float _moveSpeed;

    private bool CanUseAgent =>
        _view.Agent != null &&
        _view.Agent.enabled &&
        _view.Agent.isOnNavMesh;

    public MovementComponent(CharacterView view,
                             float moveSpeed) {
        _view = view;
        _moveSpeed = moveSpeed;

        Reset();
    }

    public void Reset() {
        if (_view.Agent == null) return;

        _view.Agent.speed = _moveSpeed;
        _view.Agent.stoppingDistance =
            POSITION_STOPPING_DISTANCE;

        if (!CanUseAgent) return;

        _view.Agent.ResetPath();
        _view.Agent.isStopped = true;
    }
    
    public void MoveToAttackRange(Vector3 position, Range attackDistanceRange) {
        float stoppingDistance =
            Mathf.Max(0f, attackDistanceRange.Max -
                          ATTACK_DISTANCE_BUFFER);
        MoveToDistance(position, stoppingDistance, 1f);
    }

    public void MoveToPosition(Vector3 position) {
        MoveToPosition(position, 1f);
    }

    public void MoveToPosition(Vector3 position, float speedMultiplier) {
        MoveToDistance(position, POSITION_STOPPING_DISTANCE,
                       speedMultiplier);
    }

    public void MoveToDistance(
        Vector3 position,
        float stoppingDistance,
        float speedMultiplier) {

        if (!CanUseAgent) return;

        _view.Agent.speed = _moveSpeed * Mathf.Max(0f, speedMultiplier);
        _view.Agent.stoppingDistance = Mathf.Max(0f, stoppingDistance);

        _view.Agent.isStopped = false;
        _view.Agent.SetDestination(position);
    }

    public void Stop() {
        if (!CanUseAgent) return;
        _view.Agent.speed = _moveSpeed;
        _view.Agent.isStopped = true;
        _view.Agent.ResetPath();
    }
}
