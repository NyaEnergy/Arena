using UnityEngine;
using UnityEngine.AI;

public class CharacterNavRouteService {
    private const float STOPPING_DISTANCE = 0.15f;

    public bool BeginAt(CharacterView view,
                        Vector3 startPosition,
                        Vector3 destination) {

        NavMeshAgent agent = GetAgent(view);

        if (agent == null ||
           !agent.Warp(startPosition)) return false;

        return Start(agent, destination);
    }

    public bool BeginFromCurrent(CharacterView view, Vector3 destination) {
        NavMeshAgent agent = GetAgent(view);

        if (agent == null ||
           !agent.isOnNavMesh)
                return false;

        return Start(agent, destination);
    }

    public CharacterNavRouteState Tick(CharacterView view) {
        NavMeshAgent agent = view?.Agent;

        if (agent == null ||
            !agent.enabled ||
            !agent.isOnNavMesh)
                return CharacterNavRouteState.Failed;

        if (agent.pathPending)
            return CharacterNavRouteState.Running;

        if (agent.pathStatus != NavMeshPathStatus.PathComplete) {
            Stop(agent);
            return CharacterNavRouteState.Failed;
        }

        if (agent.remainingDistance > STOPPING_DISTANCE)
            return CharacterNavRouteState.Running;

        Stop(agent);
        return CharacterNavRouteState.Completed;
    }

    public void Cancel(CharacterView view) {
        NavMeshAgent agent = view?.Agent;

        if (agent == null ||
            !agent.enabled ||
            !agent.isOnNavMesh) return;

        Stop(agent);
    }

    private NavMeshAgent GetAgent(CharacterView view) {
        if (view?.Agent == null) return null;

        view.SetNavigationEnabled(true);

        return view.Agent.enabled ?
               view.Agent : null;
    }

    private bool Start(NavMeshAgent agent,
                       Vector3 destination) {
        agent.stoppingDistance = STOPPING_DISTANCE;

        agent.isStopped = false;

        if (agent.SetDestination(destination))
            return true;

        Stop(agent);
        return false;
    }

    private void Stop(NavMeshAgent agent) {
        agent.isStopped = true;
        agent.ResetPath();
    }
}