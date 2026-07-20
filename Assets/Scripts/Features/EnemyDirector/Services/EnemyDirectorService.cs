using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class EnemyDirectorService : IInitializable,
                                           ITickable {
    private const float DEFAULT_FEED_INTERVAL = 4f;

    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly EnemyDirectorConfig _config;
    private readonly EnemyDirectorRuntime _runtime;

    public EnemyDirectorState State => _runtime.State;

    public float FeedInterval {
        get {
            EnemyDirectorProfile profile =
                _config?.GetProfile(State);

            return profile?.FeedInterval
                   ?? DEFAULT_FEED_INTERVAL;
        }
    }

    public EnemyDirectorService(
                BattlefieldRegistry battlefieldRegistry,
                EnemyDirectorConfig config,
                EnemyDirectorRuntime runtime) {

        _battlefieldRegistry = battlefieldRegistry;
        _config = config;
        _runtime = runtime;
    }

    public void Initialize() {
        if (_config == null) return;

        _runtime.Reset(Time.time, _config.EvaluationInterval);
    }

    public void Tick() {
        if (_config == null ||
            !_runtime.IsEvaluationReady(Time.time)) {
            return;
        }

        _runtime.ScheduleEvaluation(
            Time.time, _config.EvaluationInterval);

        EvaluateState();
    }

    private void EvaluateState() {
        float allyThreat = CalculateThreat(_battlefieldRegistry.Allies);
        float enemyThreat = CalculateThreat(_battlefieldRegistry.Enemies);

        EnemyDirectorState nextState = allyThreat > 0f &&
                                       enemyThreat < allyThreat ?
                                            EnemyDirectorState.Pressure :
                                            EnemyDirectorState.Calm;

        if (!_runtime.SetState(nextState) ||
            !_config.LogStateChanges) {
            return;
        }

        Debug.Log($"[EnemyDirector] {nextState} | " +
                  $"Ally Threat: {allyThreat:0.##} | " +
                  $"Enemy Threat: {enemyThreat:0.##}");
    }

    private float CalculateThreat( IReadOnlyList<CharacterBrain> characters) {
        float totalThreat = 0f;

        for (int i = 0; i < characters.Count; i++) {
            CharacterBrain brain = characters[i];

            if (brain?.Runtime == null ||
                brain.Config == null ||
                brain.Runtime.IsDead.CurrentValue) {
                continue;
            }

            totalThreat +=
                Mathf.Max(0.1f,
                          brain.Config.ThreatWeight);
        }

        return totalThreat;
    }
}
