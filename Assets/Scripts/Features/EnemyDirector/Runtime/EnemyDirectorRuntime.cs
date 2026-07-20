public sealed class EnemyDirectorRuntime {
    public float AllyThreat { get; private set; }
    public float EnemyThreat { get; private set; }
    public float TargetEnemyThreat { get; private set; }
    public float ThreatLoad { get; private set; }
    public float NextEvaluationTime { get; private set; }
    public bool IsRefillPaused { get; private set; }

    public void Reset(float time,
                      float evaluationInterval) {
        AllyThreat = 0f;
        EnemyThreat = 0f;
        TargetEnemyThreat = 0f;
        ThreatLoad = 1f;
        IsRefillPaused = true;

        ScheduleEvaluation(time, evaluationInterval);
    }

    public bool IsEvaluationReady(float time) {
        return time >= NextEvaluationTime;
    }

    public void ScheduleEvaluation(float time,
                                   float interval) {
        NextEvaluationTime = time + interval;
    }

    public bool UpdateThreat(float allyThreat,
                             float enemyThreat,
                             float targetEnemyThreat) {
        bool wasPaused = IsRefillPaused;

        AllyThreat = System.Math.Max(0f, allyThreat);
        EnemyThreat = System.Math.Max(0f, enemyThreat);
        TargetEnemyThreat = System.Math.Max(0f, targetEnemyThreat);

        ThreatLoad = TargetEnemyThreat > 0f ?
                     EnemyThreat / TargetEnemyThreat : 1f;

        IsRefillPaused = TargetEnemyThreat <= 0f ||
                         EnemyThreat >= TargetEnemyThreat;

        return wasPaused != IsRefillPaused;
    }
}