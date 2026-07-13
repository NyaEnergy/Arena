using System.Collections.Generic;
using UnityEngine;

public class SummonerRuntime {
    private readonly List<CharacterView> _summonedCharacters = new();

    private int _spawnIndex;

    public int SpawnIndex => _spawnIndex;

    public float NextSummonTime { get; private set; } = float.NegativeInfinity;

    public void Reset() {
        KillOwnedCharacters();

        _summonedCharacters.Clear();
        _spawnIndex = 0;

        NextSummonTime = float.NegativeInfinity;
    }

    public void Register(CharacterView character,
                         float currentTime,
                         float cooldown) {
        if (character == null) return;

        _summonedCharacters.Add(character);
        _spawnIndex++;

        NextSummonTime = currentTime +
                         Mathf.Max(0f, cooldown);
    }

    public bool HasFreeSlot(int maxSummons) {
        Clean();

        return maxSummons > 0 &&
               _summonedCharacters.Count < maxSummons;
    }

    public bool IsReady(float currentTime) {
        return currentTime >= NextSummonTime;
    }

    public void Clean() {
        for (int i = _summonedCharacters.Count - 1; i >= 0; i--) {
            if (IsUnavailable(_summonedCharacters[i])) {
                _summonedCharacters.RemoveAt(i);
            }
        }
    }

    private void KillOwnedCharacters() {
        for (int i = 0; i < _summonedCharacters.Count; i++) {
            CharacterView character = _summonedCharacters[i];

            if (IsUnavailable(character)) continue;

            float hp = character
                      .Brain
                      .Runtime
                      .CurrentHP
                      .CurrentValue;

            if (hp > 0f) {
                character.Brain
                         .HealthComponent
                         .ApplyDamage(hp);
            }
        }
    }

    private bool IsUnavailable(
        CharacterView character) {
        return character == null ||
               !character.gameObject.activeInHierarchy ||
               character.Brain == null ||
               character.Brain
                        .Runtime
                        .IsDead
                        .CurrentValue;
    }
}