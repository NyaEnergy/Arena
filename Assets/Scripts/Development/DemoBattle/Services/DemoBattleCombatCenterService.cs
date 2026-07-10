using System.Collections.Generic;
using UnityEngine;

public class DemoBattleCombatCenterService {
    private readonly BattlefieldRegistry _battlefieldRegistry;

    public DemoBattleCombatCenterService(BattlefieldRegistry battlefieldRegistry) {
        _battlefieldRegistry = battlefieldRegistry;
    }

    public bool TryGetCenter(out Vector3 center) {
        return TryGetCenterAndRadius(out center, out _);
    }

    public bool TryGetCenterAndRadius(out Vector3 center,
                                      out float radius) {
        Vector3 sum = Vector3.zero;
        int count = 0;

        IReadOnlyList<CharacterBrain> allies =
            _battlefieldRegistry.GetAllies(TeamType.Ally);

        IReadOnlyList<CharacterBrain> enemies =
            _battlefieldRegistry.GetEnemies(TeamType.Ally);

        AddCharacters(allies, ref sum, ref count);
        AddCharacters(enemies, ref sum, ref count);

        if (count <= 0) {
            center = Vector3.zero;
            radius = 0f;
            return false;
        }

        center = sum / count;
        radius = CalculateRadius(center, allies, enemies);

        return true;
    }

    private void AddCharacters(IReadOnlyList<CharacterBrain> characters,
                               ref Vector3 sum,
                               ref int count) {
        for (int i = 0; i < characters.Count; i++) {
            CharacterBrain character =
                characters[i];

            if (!CanUseCharacter(character)) continue;

            sum += character.View.transform.position;
            count++;
        }
    }

    private float CalculateRadius(Vector3 center,
                                  IReadOnlyList<CharacterBrain> allies,
                                  IReadOnlyList<CharacterBrain> enemies) {
        float maxSqrDistance = 0f;

        maxSqrDistance =
            GetMaxSqrDistance(center, allies, maxSqrDistance);

        maxSqrDistance =
            GetMaxSqrDistance(center, enemies, maxSqrDistance);

        return Mathf.Sqrt(maxSqrDistance);
    }

    private float GetMaxSqrDistance(Vector3 center,
                                    IReadOnlyList<CharacterBrain> characters,
                                    float currentMaxSqrDistance) {
        float maxSqrDistance = currentMaxSqrDistance;

        for (int i = 0; i < characters.Count; i++) {
            CharacterBrain character = characters[i];

            if (!CanUseCharacter(character)) continue;

            Vector3 position =
                character.View.transform.position;

            position.y = center.y;

            float sqrDistance =
                Vector3.SqrMagnitude(position - center);

            if (sqrDistance > maxSqrDistance) {
                maxSqrDistance = sqrDistance;
            }
        }

        return maxSqrDistance;
    }

    private bool CanUseCharacter(CharacterBrain character) {
        if (character == null ||
            character.View == null ||
            character.Runtime == null) {

            return false;
        }

        return !character.Runtime.IsDead.CurrentValue;
    }
}