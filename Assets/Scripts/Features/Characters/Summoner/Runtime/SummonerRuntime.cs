using System.Collections.Generic;

public class SummonerRuntime {
    private readonly List<CharacterView> _minions = new();

    private int _spawnIndex;

    public float NextSummonTime;

    public void Reset() {
        KillMinions();

        _minions.Clear();
        _spawnIndex = 0;
        NextSummonTime = float.NegativeInfinity;
    }

    public void AddMinion(CharacterView minion) {
        if (minion == null) return;

        _minions.Add(minion);
    }

    public bool HasFreeMinionSlot(int maxMinions) {
        if (maxMinions <= 0) return false;

        CleanMinions();

        return _minions.Count < maxMinions;
    }

    public int GetNextSpawnIndex() {
        int currentIndex = _spawnIndex;
        _spawnIndex++;
        return currentIndex;
    }

    public void CleanMinions() {
        for (int i = _minions.Count - 1; i >= 0; i--) {
            CharacterView minion = _minions[i];

            if (minion == null ||
                !minion.gameObject.activeInHierarchy ||
                minion.Brain == null ||
                minion.Brain.Runtime.IsDead.CurrentValue) {

                _minions.RemoveAt(i);
            }
        }
    }

    public void KillMinions() {
        for (int i = 0; i < _minions.Count; i++) {
            CharacterView minion = _minions[i];

            if (minion == null ||
                minion.Brain == null ||
                minion.Brain.Runtime.IsDead.CurrentValue) {

                continue;
            }

            float currentHP =
                minion.Brain.Runtime.CurrentHP.CurrentValue;

            minion.Brain.HealthComponent.ApplyDamage(currentHP);
        }
    }
}