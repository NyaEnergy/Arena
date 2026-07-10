using UnityEngine;

public class SummonerAIBehavior : ICharacterAIBehavior {
    private const float SUMMON_DISTANCE_BUFFER = 0.2f;
    private const float MIN_DIRECTION_SQR_MAGNITUDE = 0.001f;

    private readonly CharacterBrain _brain;
    private readonly SummonerConfig _config;
    private readonly DetectionService _detectionService;
    private readonly SummonerMinionSpawnService _spawnService;

    private readonly SummonerRuntime _runtime = new();

    public SummonerAIBehavior(CharacterBrain brain,
                              SummonerConfig config,
                              DetectionService detectionService,
                              SummonerMinionSpawnService spawnService) {
        _brain = brain;
        _config = config;
        _detectionService = detectionService;
        _spawnService = spawnService;

        Reset();
    }

    public void Reset() {
        _runtime.Reset();

        _brain.TargetComponent.ClearTarget();
        _brain.MovementComponent.Stop();
    }

    public void Tick() {
        _runtime.CleanMinions();

        CharacterBrain target =
            _detectionService.FindClosestTarget(_brain);

        _brain.TargetComponent.SetTarget(target);

        if (target == null) {
            _brain.MovementComponent.Stop();
            return;
        }

        float sqrDistance =
            Vector3.SqrMagnitude(
                _brain.View.transform.position -
                target.View.transform.position);

        Range summonDistanceRange =
            _config.SummonDistanceRange;

        Range sqrSummonDistanceRange = new(
            summonDistanceRange.Min * summonDistanceRange.Min,
            summonDistanceRange.Max * summonDistanceRange.Max);

        if (sqrDistance > sqrSummonDistanceRange.Max) {
            MoveToSummonDistance(target);
            return;
        }

        if (sqrDistance < sqrSummonDistanceRange.Min) {
            MoveAwayFromTarget(target);
            return;
        }

        _brain.MovementComponent.Stop();

        TrySummon(target);
    }

    private void MoveToSummonDistance(CharacterBrain target) {
        float stoppingDistance =
            Mathf.Max(0f, _config.SummonDistanceRange.Max -
                          SUMMON_DISTANCE_BUFFER);

        _brain.MovementComponent.MoveToDistance(
            target.View.transform.position,
            stoppingDistance,
            1f);
    }

    private void MoveAwayFromTarget(CharacterBrain target) {
        Vector3 currentPosition =
            _brain.View.transform.position;

        Vector3 awayDirection =
            currentPosition - target.View.transform.position;

        awayDirection.y = 0f;

        if (awayDirection.sqrMagnitude < MIN_DIRECTION_SQR_MAGNITUDE) {
            _brain.MovementComponent.Stop();
            return;
        }

        Vector3 retreatPosition =
            currentPosition +
            awayDirection.normalized * _config.RetreatStepDistance;

        _brain.MovementComponent.MoveToPosition(retreatPosition);
    }

    private void TrySummon(CharacterBrain target) {
        if (Time.time < _runtime.NextSummonTime) return;

        if (!_runtime.HasFreeMinionSlot(_config.MaxMinions)) return;

        int spawnIndex =
            _runtime.GetNextSpawnIndex();

        CharacterView minion =
            _spawnService.SpawnMinion(_brain, target, spawnIndex);

        if (minion == null) return;

        _runtime.AddMinion(minion);

        _runtime.NextSummonTime =
            Time.time + _config.SummonCooldown;

        SummonerView summonerView =
            _brain.View as SummonerView;

        summonerView?.PlaySummon();

        MinionView minionView =
            minion as MinionView;

        minionView?.PlaySpawn();
    }
}