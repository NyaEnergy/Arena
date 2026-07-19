using System.Collections.Generic;

public class TerritoryHeroService {
    private readonly BattlefieldRegistry _battlefield;

    public TerritoryHeroService(BattlefieldRegistry battlefield) {
        _battlefield = battlefield;
    }

    public bool HasLivingAlly(TerritoryRuntime territory) {
        if (territory?.View == null) return false;

        IReadOnlyList<CharacterBrain> allies = _battlefield.Allies;

        for (int i = 0; i < allies.Count; i++) {
            CharacterBrain ally = allies[i];

            if (!IsLivingHero(ally)) continue;

            if (territory.View.Contains(ally.View.transform.position)) 
                return true;
        }

        return false;
    }

    private bool IsLivingHero(CharacterBrain brain) {
        return brain != null &&
               brain.View != null &&
               brain.Runtime != null &&
               !brain.Runtime.IsDead.CurrentValue &&
               brain.Config is ICharacterConfig;
    }
}