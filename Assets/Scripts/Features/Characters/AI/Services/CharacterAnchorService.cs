using System.Collections.Generic;

public class CharacterAnchorService {
    private readonly CharacterTeamService _teamService;

    public CharacterAnchorService(CharacterTeamService teamService) {
        _teamService = teamService;
    }

    public bool TryGet(TeamType teamType,
                   out CharacterBrain anchor) {

        IReadOnlyList<CharacterBrain> members =
            _teamService.GetMembers(teamType);

        anchor = FindMobileFront(members);
        if (anchor != null) return true;

        anchor = FindMobile(members);
        if (anchor != null) return true;

        anchor = FindLiving(members);
        return anchor != null;
    }

    public int GetMemberIndex(CharacterBrain brain,
                              CharacterBrain anchor) {

        IReadOnlyList<CharacterBrain> members =
            _teamService.GetMembers(brain.Runtime.TeamType);

        int index = 1;

        for (int i = 0; i < members.Count; i++) {
            CharacterBrain current = members[i];

            if (!_teamService.IsAlive(current) ||
                current == anchor) {
                continue;
            }

            if (current == brain) return index;

            index++;
        }

        return index;
    }

    private CharacterBrain FindMobileFront(IReadOnlyList<CharacterBrain> members) {
        for (int i = 0; i < members.Count; i++) {
            CharacterBrain member = members[i];

            if (_teamService.IsMobile(member) &&
                member.Config.CombatRow ==
                CharacterCombatRow.Front) {
                return member;
            }
        }

        return null;
    }

    private CharacterBrain FindMobile(IReadOnlyList<CharacterBrain> members) {
        for (int i = 0; i < members.Count; i++) {
            if (_teamService.IsMobile(members[i])) {
                return members[i];
            }
        }

        return null;
    }

    private CharacterBrain FindLiving(IReadOnlyList<CharacterBrain> members) {
        for (int i = 0; i < members.Count; i++) {
            if (_teamService.IsAlive(members[i])) {
                return members[i];
            }
        }

        return null;
    }
}