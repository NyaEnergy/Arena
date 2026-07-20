using UnityEngine;

public class MedicCombatService {
    private const float ATTACK_DISTANCE_BUFFER = 0.2f;

    private readonly MedicConfig _config;
    private readonly DetectionService _detectionService;
    private readonly CharacterLineOfSightService _lineOfSightService;
    private readonly EnemyDirectorService _directorService;

    public MedicCombatService(MedicConfig config,
                              DetectionService detectionService,
                              CharacterLineOfSightService lineOfSightService,
                              EnemyDirectorService directorService) {

        _config = config;
        _detectionService = detectionService;
        _lineOfSightService = lineOfSightService;
        _directorService = directorService;
    }

    public void Tick(CharacterBrain medic,
                     MedicCombatRuntime runtime) {
        CharacterBrain target =
            _detectionService.FindClosestTarget(medic);

        medic.TargetComponent.SetTarget(target);

        if (target == null) {
            medic.MovementComponent.Stop();
            return;
        }

        Vector3 medicPosition = medic.View.transform.position;
        Vector3 targetPosition = target.View.transform.position;

        float sqrDistance =
            Vector3.SqrMagnitude(
                medicPosition - targetPosition);

        Range attackDistanceRange = _config.AttackDistanceRange;

        Range sqrAttackDistanceRange = new(
            attackDistanceRange.Min * attackDistanceRange.Min,
            attackDistanceRange.Max * attackDistanceRange.Max);

        if (sqrDistance > sqrAttackDistanceRange.Max) {
            MoveToAttackDistance(medic, targetPosition);
            return;
        }

        if (sqrDistance < sqrAttackDistanceRange.Min) {
            medic.MovementComponent.Stop();
            return;
        }

        if (!_lineOfSightService.HasClearShot(medic, target,
                                              _config.LineOfSightBlockingLayers,
                                              _config.LineOfSightTriggerInteraction)) {
            medic.MovementComponent.MoveToPosition(targetPosition);
            return;
        }

        medic.MovementComponent.Stop();
        TryAttack(medic, target, runtime);
    }

    private void MoveToAttackDistance(CharacterBrain medic,
                                      Vector3 targetPosition) {
        float stoppingDistance =
            Mathf.Max(0f, _config.AttackDistanceRange.Max -
                          ATTACK_DISTANCE_BUFFER);

        medic.MovementComponent
             .MoveToDistance(targetPosition,
                             stoppingDistance, 1f);
    }

    private void TryAttack(CharacterBrain medic,
                           CharacterBrain target,
                           MedicCombatRuntime runtime) {
        if (target.Runtime.IsDead.CurrentValue ||
            Time.time < runtime.NextAttackTime) {
            return;
        }

        float damage =
            _directorService != null ?
            _directorService.GetDamage(medic,
                                       target,
                                      _config.Damage)
            : _config.Damage;

        target.HealthComponent.ApplyDamage(damage);

        runtime.NextAttackTime = Time.time +
                                 _config.AttackCooldown;

        medic.View.PlayAttack(target.View);
    }
}