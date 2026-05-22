using UnityEngine;
using UnityEngine.AI;

public class MovementComponent {
    private readonly NavMeshAgent _agent;

    public MovementComponent(NavMeshAgent agent) {
        _agent = agent;
    }

    public void MoveTo(Vector3 position) {
        _agent.SetDestination(position);
    }

    public void Stop() {
        _agent.ResetPath();
    }
}
