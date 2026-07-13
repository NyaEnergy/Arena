using System.Collections.Generic;
using UnityEngine;

public class HunterVanguardQueryService {
    private readonly BattlefieldRegistry _battlefieldRegistry;
    private readonly HunterConfig _config;

    public HunterVanguardQueryService(BattlefieldRegistry battlefieldRegistry,
                                      HunterConfig config) {

        _battlefieldRegistry = battlefieldRegistry;
        _config = config;
    }

    public CharacterBrain FindClosest(CharacterBrain hunter) {
        IReadOnlyList<CharacterBrain> allies =
            _battlefieldRegistry.GetAllies(
                hunter.Runtime.TeamType);

        CharacterBrain closestVanguard = null;

        float closestSqrDistance =
            _config.VanguardSearchDistance *
            _config.VanguardSearchDistance;

        Vector3 hunterPosition =
            hunter.View.transform.position;

        for (int i = 0; i < allies.Count; i++) {
            CharacterBrain candidate = allies[i];

            if (!IsAvailableVanguard(hunter, candidate)) continue;

            float sqrDistance =
                Vector3.SqrMagnitude(
                    hunterPosition -
                    candidate.View.transform.position);

            if (sqrDistance > closestSqrDistance) continue;

            closestSqrDistance = sqrDistance;
            closestVanguard = candidate;
        }

        return closestVanguard;
    }

    private bool IsAvailableVanguard(CharacterBrain hunter,
                                     CharacterBrain candidate) {
        return candidate != null &&
               candidate != hunter &&
               !candidate.Runtime.IsDead.CurrentValue &&
               candidate.Runtime.TeamType == hunter.Runtime.TeamType &&
               candidate.Config is VanguardConfig;
    }
}