using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class DemoBattleAutoRespawnService : IInitializable,
                                            ITickable,
                                            IDisposable {
    private const float RESPAWN_DELAY = 1f;
    private const float RESPAWN_DISTANCE_FROM_CENTER = 3.5f;
    private const float RESPAWN_RANDOM_RADIUS = 1.2f;
    private const float NAV_MESH_SAMPLE_DISTANCE = 3f;
    private const float MIN_DIRECTION_SQR_MAGNITUDE = 0.001f;

    private readonly IReadOnlyList<DemoBattleSpawnEntry> _spawnEntries;
    private readonly CharacterDeathEventService _deathEventService;
    private readonly CharacterFactory _characterFactory;
    private readonly DemoBattleCombatCenterService _combatCenterService;
    private readonly List<DemoBattleAutoRespawnRequest> _requests = new();
    private readonly System.Random _random = new();

    public DemoBattleAutoRespawnService(IReadOnlyList<DemoBattleSpawnEntry> spawnEntries,
                                        CharacterDeathEventService deathEventService,
                                        CharacterFactory characterFactory,
                                        DemoBattleCombatCenterService combatCenterService) {
        _spawnEntries = spawnEntries;
        _deathEventService = deathEventService;
        _characterFactory = characterFactory;
        _combatCenterService = combatCenterService;
    }

    public void Initialize() {
        _deathEventService.CharacterDied += OnCharacterDied;
    }

    public void Tick() {
        for (int i = _requests.Count - 1; i >= 0; i--) {
            DemoBattleAutoRespawnRequest request = _requests[i];

            request.RemainingTime -= Time.deltaTime;

            if (request.RemainingTime > 0f) {
                _requests[i] = request;
                continue;
            }

            SpawnRandomCharacter(request.TeamType);

            _requests.RemoveAt(i);
        }
    }

    public void Dispose() {
        _deathEventService.CharacterDied -= OnCharacterDied;
        _requests.Clear();
    }

    private void OnCharacterDied(CharacterDeathInfo deathInfo) {

        if (deathInfo.CharacterType ==
            CharacterType.Minion) return;

        if (!HasRespawnSource(deathInfo.TeamType,
                              deathInfo.CharacterType)) return;

        DemoBattleAutoRespawnRequest request =
            new(deathInfo.TeamType,
                RESPAWN_DELAY);

        _requests.Add(request);
    }

    private bool HasRespawnSource(TeamType teamType,
                                  CharacterType characterType) {

        for (int i = 0; i < _spawnEntries.Count; i++) {
            DemoBattleSpawnEntry spawnEntry = _spawnEntries[i];

            if (!CanUseForAutoRespawn(spawnEntry)) continue;
            if (spawnEntry.TeamType != teamType) continue;
            if (spawnEntry.CharacterType != characterType) continue;

            return true;
        }

        return false;
    }

    private void SpawnRandomCharacter(
        TeamType teamType) {

        DemoBattleSpawnEntry spawnEntry =
            GetRandomSpawnEntry(teamType);

        if (spawnEntry == null) return;

        Vector3 spawnPosition =
            GetRespawnPosition(spawnEntry);

        CharacterKey characterKey =
            new(spawnEntry.TeamType,
                spawnEntry.CharacterType);

        CharacterView character =
            _characterFactory.Spawn(
                characterKey,
                spawnPosition);

        if (character == null) return;

        character.transform.rotation =
            spawnEntry.SpawnPoint.rotation;
    }

    private Vector3 GetRespawnPosition(
        DemoBattleSpawnEntry spawnEntry) {

        if (spawnEntry.SpawnPoint == null) return Vector3.zero;

        if (!_combatCenterService.TryGetCenter(out Vector3 center))
            return spawnEntry.SpawnPoint.position;

        Vector3 direction = spawnEntry.SpawnPoint.position - center;

        direction.y = 0f;

        if (direction.sqrMagnitude <
            MIN_DIRECTION_SQR_MAGNITUDE) {

            direction = spawnEntry.TeamType == TeamType.Ally ?
                        Vector3.left : Vector3.right;
        }

        direction.Normalize();

        Vector3 randomOffset = GetRandomOffset();

        Vector3 position = center +
                           direction * RESPAWN_DISTANCE_FROM_CENTER +
                           randomOffset;

        position.y = spawnEntry.SpawnPoint.position.y;

        if (NavMesh.SamplePosition(position,
                                   out NavMeshHit hit,
                                   NAV_MESH_SAMPLE_DISTANCE,
                                   NavMesh.AllAreas)) {
            return hit.position;
        }

        return spawnEntry.SpawnPoint.position;
    }

    private Vector3 GetRandomOffset() {
        double angle = _random.NextDouble() * Math.PI * 2.0;
        double distance = _random.NextDouble() * RESPAWN_RANDOM_RADIUS;

        float x = (float)(Math.Cos(angle) * distance);
        float z = (float)(Math.Sin(angle) * distance);

        return new Vector3(x, 0f, z);
    }

    private DemoBattleSpawnEntry GetRandomSpawnEntry(TeamType teamType) {

        int validCount = CountValidSpawnEntries(teamType);

        if (validCount <= 0) return null;

        int targetIndex = _random.Next(validCount);

        int currentIndex = 0;

        for (int i = 0; i < _spawnEntries.Count; i++) {
            DemoBattleSpawnEntry spawnEntry = _spawnEntries[i];

            if (!CanUseForAutoRespawn(spawnEntry)) continue;
            if (spawnEntry.TeamType != teamType) continue;

            if (currentIndex == targetIndex) return spawnEntry;

            currentIndex++;
        }

        return null;
    }

    private int CountValidSpawnEntries(TeamType teamType) {

        int count = 0;

        for (int i = 0; i < _spawnEntries.Count; i++) {
            DemoBattleSpawnEntry spawnEntry = _spawnEntries[i];

            if (!CanUseForAutoRespawn(spawnEntry)) continue;
            if (spawnEntry.TeamType != teamType) continue;

            count++;
        }

        return count;
    }

    private bool CanUseForAutoRespawn(DemoBattleSpawnEntry spawnEntry) {
        return spawnEntry != null &&
               spawnEntry.IsValid &&
               spawnEntry.CharacterType !=
               CharacterType.Minion;
    }
}