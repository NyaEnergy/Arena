public sealed class EnemyConveyorRuntime {
    private float _remainingStartDelay;
    private float _feedProgress;
    private bool _isFirstFeedPending;

    public int NextGroupIndex { get; private set; }

    public void Reset(float startDelay) {
        _remainingStartDelay = UnityEngine.Mathf.Max(0f, startDelay);

        _feedProgress = 0f;
        _isFirstFeedPending = true;
        NextGroupIndex = 0;
    }

    public void Tick(float deltaTime) {
        _remainingStartDelay = UnityEngine.Mathf.Max(
            0f, _remainingStartDelay -
                 UnityEngine.Mathf.Max(0f, deltaTime));
    }

    public bool TryConsumeFeed(float deltaTime,
                               float feedInterval) {
        if (_remainingStartDelay > 0f) return false;

        if (_isFirstFeedPending) {
            _isFirstFeedPending = false;
            return true;
        }

        float safeDeltaTime =
            UnityEngine.Mathf.Max(0f, deltaTime);

        _feedProgress += safeDeltaTime /
            UnityEngine.Mathf.Max(0.1f, feedInterval);

        if (_feedProgress < 1f) return false;

        _feedProgress = 0f;
        return true;
    }

    public void ConfirmGroup(int usedIndex,
                             int groupCount) {
        NextGroupIndex = groupCount > 0 ?
                        (usedIndex + 1) % groupCount : 0;
    }
}
