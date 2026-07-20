using System.Collections.Generic;
using UnityEngine;
using Zenject;

public sealed class EnemyDirectorService : IInitializable,
                                           ITickable {
    private const float DEFAULT_FEED_INTERVAL = 4f;

    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly EnemyCommanderConfig _config;
    private readonly EnemyDirectorRuntime _runtime;

    public bool CanRefill => !_runtime.IsRefillPaused;
    public float ThreatLoad => _runtime.ThreatLoad;
    public float FeedInterval => _config != null ?
                                 _config.GetRefillInterval(ThreatLoad) :
                                 DEFAULT_FEED_INTERVAL;

    public EnemyDirectorService(BattlefieldRegistry battlefieldRegistry,
                                EnemyCommanderConfig config,
                                EnemyDirectorRuntime runtime) {

        _battlefieldRegistry = battlefieldRegistry;
        _config = config;
        _runtime = runtime;
    }

    public void Initialize() {
        if (_config == null) return;

        _runtime.Reset(Time.time, _config.EvaluationInterval);
        EvaluateThreat();
    }

    public void Tick() {
        if (_config == null ||
            !_runtime.IsEvaluationReady(Time.time)) {
            return;
        }

        _runtime.ScheduleEvaluation(
            Time.time, _config.EvaluationInterval);

        EvaluateThreat();
    }

    public float GetDamage(CharacterBrain attacker,
                           CharacterBrain target,
                           float damage) {
        float result = Mathf.Max(0f, damage);

        if (_config == null ||
            attacker?.Runtime == null ||
            target?.Runtime == null) {
            return result;
        }

        if (attacker.Runtime.TeamType == TeamType.Enemy &&
            target.Runtime.TeamType == TeamType.Ally) {
            result *= _config.EnemyToAllyDamageMultiplier;
        }

        if (target.Runtime.TeamType == TeamType.Enemy &&
            target.Config != null) {
            float maximumDamage =
                target.Config.MaxHP *
                _config.MaximumEnemyHealthFractionPerHit;

            result = Mathf.Min(result, maximumDamage);
        }

        return result;
    }

    private void EvaluateThreat() {
        float allyThreat = CalculateThreat(_battlefieldRegistry.Allies);
        float enemyThreat = CalculateThreat(_battlefieldRegistry.Enemies);

        float targetEnemyThreat =
            allyThreat * _config.TargetEnemyThreatRatio;

        bool pauseChanged = _runtime.UpdateThreat(
            allyThreat,
            enemyThreat,
            targetEnemyThreat);

        if (!pauseChanged ||
            !_config.LogRefillChanges) {
            return;
        }

        string refillState = _runtime.IsRefillPaused
            ? "Paused"
            : "Active";

        Debug.Log($"[EnemyDirector] Refill {refillState} | " +
                  $"Ally Threat: {allyThreat:0.##} | " +
                  $"Enemy Threat: {enemyThreat:0.##}/" +
                  $"{targetEnemyThreat:0.##} | " +
                  $"Load: {_runtime.ThreatLoad:P0}");
    }

    private float CalculateThreat(IReadOnlyList<CharacterBrain> characters) {
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
