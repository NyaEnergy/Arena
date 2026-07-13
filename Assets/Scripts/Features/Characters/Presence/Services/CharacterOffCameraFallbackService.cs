using UnityEngine;

public class CharacterOffCameraFallbackService {
    private readonly CharacterTeleportPresenceService _teleportService;

    public CharacterOffCameraFallbackService(
                CharacterTeleportPresenceService teleportService) {
        
        _teleportService = teleportService;
    }

    public void Play(CharacterView view,
                     CharacterOffCameraRoutePresenceConfig config,
                     CharacterPresenceTransitionRequest source,
                     Vector3 position) {
        
        if (view == null) return;

        CharacterPresenceTransitionRequest request =
            new(source.Direction,
                view.transform.position,
                position,
                view.transform.rotation,
                source.EndRotation
            );

        _teleportService.Begin(view,
                               config?.FallbackTeleport,
                               request);
    }
}