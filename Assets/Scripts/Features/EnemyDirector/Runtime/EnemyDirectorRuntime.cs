public sealed class EnemyDirectorRuntime {
    public EnemyDirectorState State { get; private set; }

    public float NextEvaluationTime { get; private set; }

    public int NextCalmEntryIndex { get; private set; }
    public int NextPressureEntryIndex { get; private set; }

    public void Reset(float time,
                      float evaluationInterval) {

        State = EnemyDirectorState.Calm;

        NextCalmEntryIndex = 0;
        NextPressureEntryIndex = 0;

        ScheduleEvaluation(time, evaluationInterval);
    }

    public bool IsEvaluationReady(float time) {
        return time >= NextEvaluationTime;
    }

    public void ScheduleEvaluation(float time, float interval) {
        NextEvaluationTime = time + interval;
    }

    public bool SetState(EnemyDirectorState state) {
        if (State == state) return false;

        State = state;
        return true;
    }

    public int GetNextEntryIndex(EnemyDirectorState state) {
        return state == EnemyDirectorState.Pressure ?
               NextPressureEntryIndex : NextCalmEntryIndex;
    }

    public void Advance(EnemyDirectorState state,
                        int usedIndex,
                        int entryCount) {

        int nextIndex = entryCount > 0 ?
            (usedIndex + 1) % entryCount : 0;

        if (state == EnemyDirectorState.Pressure) {
            NextPressureEntryIndex = nextIndex;
            return;
        }

        NextCalmEntryIndex = nextIndex;
    }
}