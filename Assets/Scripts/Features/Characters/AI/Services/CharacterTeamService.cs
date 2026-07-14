using System.Collections.Generic;

public class CharacterTeamService {
    private readonly BattlefieldRegistry _registry;

    public CharacterTeamService(BattlefieldRegistry registry) {
        _registry = registry;
    }

    public IReadOnlyList<CharacterBrain> GetMembers(TeamType teamType) {
        return _registry.GetAllies(teamType);
    }

    public bool HasLivingOpponents(TeamType teamType) {
        IReadOnlyList<CharacterBrain> opponents =
            _registry.GetEnemies(teamType);

        for (int i = 0; i < opponents.Count; i++) {
            if (IsAlive(opponents[i])) return true;
        }

        return false;
    }

    public bool IsAlive(CharacterBrain brain) {
        return brain != null &&
               brain.View != null &&
               brain.Runtime != null &&
               !brain.Runtime.IsDead.CurrentValue;
    }

    public bool IsMobile(CharacterBrain brain) {
        return IsAlive(brain) &&
               brain.Config.MoveSpeed > 0.01f;
    }
}