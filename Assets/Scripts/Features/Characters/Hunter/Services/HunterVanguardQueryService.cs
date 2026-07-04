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
        
        float closestSqrDistance = _config.VanguardSearchDistance *
                                   _config.VanguardSearchDistance;

        Vector3 hunterPosition = hunter.View.transform.position;

        for(int i = 0; i < allies.Count; ++i) {
            CharacterBrain candidat = allies[i];
            
            if (!IsAvailableVanguard(hunter, candidat)) continue;

            float sqrDistance = Vector3.SqrMagnitude(hunterPosition - candidat.View.transform.position);

            if (sqrDistance > closestSqrDistance) continue;

            closestSqrDistance = sqrDistance;
            closestVanguard = candidat;
        }

        return closestVanguard;
    }

    private bool IsAvailableVanguard(CharacterBrain hunter,
                                     CharacterBrain candidat) {
        return candidat != null &&
               candidat != hunter &&
               !candidat.Runtime.IsDead.CurrentValue &&
               candidat.Runtime.TeamType == hunter.Runtime.TeamType &&
               candidat.Config.CharacterType == CharacterType.Vanguard;
    }
}
