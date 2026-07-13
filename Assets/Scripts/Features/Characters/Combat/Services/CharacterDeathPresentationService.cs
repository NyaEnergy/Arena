using UnityEngine;

public class CharacterDeathPresentationService {
    private const float DEFAULT_ANIMATION_DURATION = 1f;

    private const float DEFAULT_BODY_DURATION = 2.5f;

    private readonly CharacterDeathPresentationConfig _config;

    public CharacterDeathPresentationService(
                CharacterDeathPresentationConfig config) {

        _config = config;
    }

    public void Begin(CharacterDeathRuntime runtime) {
        if (runtime == null) return;

        float duration = _config != null ?
            _config.TotalDuration : DEFAULT_ANIMATION_DURATION +
                                    DEFAULT_BODY_DURATION;

        runtime.Begin(duration);
    }

    public bool Tick(CharacterDeathRuntime runtime) {
        if (runtime == null ||
            !runtime.IsActive) 
                return true;

        runtime.Advance( Time.deltaTime);
        return runtime.IsComplete;
    }
}