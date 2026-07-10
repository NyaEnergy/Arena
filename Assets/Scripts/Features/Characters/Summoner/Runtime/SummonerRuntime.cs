using System.Collections.Generic;

public class SummonerRuntime {
    private readonly List<CharacterView> _minions = new();

    private int _spawnIndex;

    public float NextSummonTime { get; private set; } = float.NegativeInfinity;

    public int SpawnIndex => _spawnIndex;

    public void Reset() {
        KillOwnedMinions();

        _minions.Clear();
        _spawnIndex = 0;
        NextSummonTime = float.NegativeInfinity;
    }

    public void RegisterMinion(CharacterView minion,
                               float currentTime,
                               float summonCooldown) {
        if (minion == null) return;

        float safeCooldown = summonCooldown > 0f ? summonCooldown : 0f;

        _minions.Add(minion);
        _spawnIndex++;

        NextSummonTime = currentTime + safeCooldown;
    }

    public bool HasFreeMinionSlot(int maxMinions) {
        if (maxMinions <= 0) return false;

        CleanMinions();

        return _minions.Count < maxMinions;
    }

    public bool IsSummonReady(float currentTime) {
        return currentTime >= NextSummonTime;
    }

    public void CleanMinions() {
        for (int i = _minions.Count - 1; i >= 0; i--) {
            CharacterView minion = _minions[i];

            if (IsMinionUnavailable(minion)) {
                _minions.RemoveAt(i);
            }
        }
    }

    private bool IsMinionUnavailable(CharacterView minion) {
        return minion == null ||
               !minion.gameObject.activeInHierarchy ||
               minion.Brain == null ||
               minion.Brain.Runtime.IsDead.CurrentValue;
    }

    private void KillOwnedMinions() {
        for (int i = 0; i < _minions.Count; i++) {
            CharacterView minion = _minions[i];

            if (IsMinionUnavailable(minion)) continue;

            float currentHP = minion.Brain.Runtime.CurrentHP.CurrentValue;

            if (currentHP <= 0f) continue;

            minion.Brain.HealthComponent.ApplyDamage(currentHP);
        }
    }
}