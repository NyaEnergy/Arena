using UnityEngine;

public class CharacterPresenceTransitionRuntime {
    public CharacterPresencePresentationConfig Config { get; private set; }
    public CharacterPresenceTransitionRequest Request { get; private set; }

    public Vector3 Destination { get; private set; }

    public CharacterArcPresenceRuntime ArcRuntime { get; } = new();
    public CharacterAirPresenceRuntime AirRuntime { get; } = new();
    public CharacterUndergroundPresenceRuntime UndergroundRuntime { get; } = new();

    public bool IsActive { get; private set; }

    public void Begin(CharacterPresencePresentationConfig config,
                      CharacterPresenceTransitionRequest request,
                      Vector3 destination) {
        Config = config;
        Request = request;
        Destination = destination;
        IsActive = true;
    }

    public void BeginArc(CharacterPresenceTransitionRequest request,
                         CharacterArcPresenceConfig config) {
        Begin(config, request, request.EndPosition);
        ArcRuntime.Begin(request, config);
    }

    public void BeginAir(CharacterPresenceTransitionRequest request,
                         CharacterAirPresenceConfig config) {
        Begin(config, request, request.EndPosition);
        AirRuntime.Begin(request, config);
    }

    public void BeginUnderground(CharacterPresenceTransitionRequest request,
                                 CharacterUndergroundPresenceConfig config) {
        Begin(config, request, request.EndPosition);
        UndergroundRuntime.Begin(request, config);
    }

    public void Complete() {
        Reset();
    }

    public void Reset() {
        Config = null;
        Request = default;
        Destination = Vector3.zero;
        IsActive = false;

        ArcRuntime.Reset();
        AirRuntime.Reset();
        UndergroundRuntime.Reset();
    }
}