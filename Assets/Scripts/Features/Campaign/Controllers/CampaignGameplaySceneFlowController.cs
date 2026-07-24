using System;
using Zenject;

public sealed class CampaignGameplaySceneFlowController : IInitializable,
                                                          ITickable,
                                                          IDisposable {
    private readonly CampaignRuntime _runtime;
    private readonly CampaignSceneFlowService _sceneFlowService;

    private bool _isReturnRequested;

    public CampaignGameplaySceneFlowController(
        CampaignRuntime runtime,
        CampaignSceneFlowService sceneFlowService) {

        _runtime = runtime ??
            throw new ArgumentNullException(nameof(runtime));
        _sceneFlowService = sceneFlowService ??
            throw new ArgumentNullException(nameof(sceneFlowService));
    }

    public void Initialize() {
        _runtime.Changed += OnRuntimeChanged;
    }

    public void Tick() {
        if (!_isReturnRequested) return;

        _isReturnRequested = false;
        _sceneFlowService.TryReturnToCampaign();
    }

    public void Dispose() {
        _runtime.Changed -= OnRuntimeChanged;
        _isReturnRequested = false;
    }

    private void OnRuntimeChanged() {
        _isReturnRequested =
            _runtime.State == CampaignState.TerritoryReady ||
            _runtime.State == CampaignState.ArcCompleted;
    }
}
