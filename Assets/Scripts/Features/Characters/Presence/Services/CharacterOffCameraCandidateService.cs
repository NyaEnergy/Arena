using UnityEngine;
using UnityEngine.AI;

public class CharacterOffCameraCandidateService {
    private const float SAMPLE_DISTANCE = 1.5f;

    private readonly CharacterNavMeshPathService _pathService;

    public CharacterOffCameraCandidateService(CharacterNavMeshPathService pathService) {
        _pathService = pathService;
    }

    public bool TryGet(CharacterOffCameraRoutePresenceConfig config,
                       Vector3 candidate,
                       Vector3 anchor,
                       CharacterPresenceTransitionDirection direction,
                   out Vector3 position) {
        
        position = anchor;

        if (config == null) return false;

        if (!NavMesh.SamplePosition(candidate,
                                out NavMeshHit hit,
                                    SAMPLE_DISTANCE,
                                    NavMesh.AllAreas)) {
            return false;
        }

        Vector3 pathStart = direction == CharacterPresenceTransitionDirection.Enter ?
            hit.position : anchor;

        Vector3 pathEnd = direction == CharacterPresenceTransitionDirection.Enter ?
            anchor : hit.position;

        if (!_pathService.HasCompletePath(pathStart,
                                          pathEnd,
                                          config.MaxPathLength)) {
            return false;
        }

        position = hit.position;
        return true;
    }
}