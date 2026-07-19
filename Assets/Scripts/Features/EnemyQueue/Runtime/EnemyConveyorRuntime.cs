public sealed class EnemyConveyorRuntime {
    public int NextEntryIndex { get; private set; }
    public float NextFeedTime { get; private set; }

    public bool IsReady(float time) {
        return time >= NextFeedTime;
    }

    public void Reset(float time, float delay) {
        NextEntryIndex = 0;
        NextFeedTime = time + delay;
    }

    public void Schedule(float time, float interval) {
        NextFeedTime = time + interval;
    }

    public void Advance(int usedIndex, int entryCount) {
        NextEntryIndex = entryCount > 0 ?
                         (usedIndex + 1) % entryCount : 0;
    }
}
