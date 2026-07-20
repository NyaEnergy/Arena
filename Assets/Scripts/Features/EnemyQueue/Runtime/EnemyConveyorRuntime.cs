public sealed class EnemyConveyorRuntime {
    public float NextFeedTime { get; private set; }

    public bool IsReady(float time) {
        return time >= NextFeedTime;
    }

    public void Reset(float time, float delay) {
        NextFeedTime = time + delay;
    }

    public void Schedule(float time, float interval) {
        NextFeedTime = time + interval;
    }
}
