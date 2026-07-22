using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Commander/Enemy Commander Config",
                 fileName = "EnemyCommanderConfig")]
public sealed class EnemyCommanderConfig : CommanderConfig {
    [Header("Queue")]
    [SerializeField, Min(0f)] private float _startDelay = 2f;
    [SerializeField, Min(0.1f)] private float _minimumRefillInterval = 2f;
    [SerializeField, Min(0.1f)] private float _maximumRefillInterval = 8f;

    [Header("Director")]
    [SerializeField, Min(0.1f)] private float _targetEnemyThreatRatio = 1.5f;
    [SerializeField, Min(0.1f)] private float _evaluationInterval = 0.5f;

    [Header("Combat")]
    [SerializeField, Range(0.1f, 1f)] private float _enemyToAllyDamageMultiplier = 0.4f;
    [SerializeField, Range(0.1f, 1f)] private float _maximumEnemyHealthFractionPerHit = 0.45f;

    [Header("Forces")]
    [SerializeField] private List<EnemyGroupConfig> _groups = new();
    [SerializeField] private EnemyEliteConfig _elite;

    [Header("Diagnostics")]
    [SerializeField] private bool _logRefillChanges;

    public override TeamType TeamType => global::TeamType.Enemy;
    public float StartDelay => Mathf.Max(0f, _startDelay);
    public float MinimumRefillInterval => Mathf.Max(0.1f, _minimumRefillInterval);
    public float MaximumRefillInterval => Mathf.Max(MinimumRefillInterval, _maximumRefillInterval);
    public float TargetEnemyThreatRatio => Mathf.Max(0.1f, _targetEnemyThreatRatio);
    public float EvaluationInterval => Mathf.Max(0.1f, _evaluationInterval);
    public float EnemyToAllyDamageMultiplier => Mathf.Clamp01(_enemyToAllyDamageMultiplier);
    public float MaximumEnemyHealthFractionPerHit => Mathf.Clamp01(_maximumEnemyHealthFractionPerHit);
    public IReadOnlyList<EnemyGroupConfig> Groups => _groups;
    public EnemyEliteConfig Elite => _elite;
    public bool LogRefillChanges => _logRefillChanges;

    public float GetRefillInterval(float threatLoad) {
        return Mathf.Lerp(
            MinimumRefillInterval,
            MaximumRefillInterval,
            Mathf.Clamp01(threatLoad));
    }
}
