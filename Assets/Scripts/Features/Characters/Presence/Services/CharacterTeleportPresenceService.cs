public class CharacterTeleportPresenceService {
    private readonly CharacterPresenceEffectService _effectService;

    public CharacterTeleportPresenceService(
            CharacterPresenceEffectService effectService) {
        
        _effectService = effectService;
    }

    public bool Begin(CharacterView view,
                      CharacterTeleportPresenceConfig config,
                      CharacterPresenceTransitionRequest request) {
        
        if (view == null) return true;

        view.transform.SetPositionAndRotation(
            request.EndPosition,
            request.EndRotation
        );

        if (request.Direction == CharacterPresenceTransitionDirection.Exit) {
            view.SetNavigationEnabled(false);
        }

        if (config != null) {
            _effectService.Play(config.EffectPrefab,
                                request.EndPosition,
                                request.EndRotation);
        }

        return true;
    }
}