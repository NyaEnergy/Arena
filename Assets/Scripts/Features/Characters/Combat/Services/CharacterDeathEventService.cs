using System;

public class CharacterDeathEventService {
    private readonly CommanderQuestService _questService;

    public event Action<CharacterDeathInfo> CharacterDied;

    public CharacterDeathEventService(CommanderQuestService questService) {
        _questService = questService;
    }

    public void NotifyDeath(CharacterDeathInfo deathInfo) {
        _questService.Report(new CommanderQuestEvent(
            CommanderQuestEventType.CharacterDefeated,
            deathInfo.TeamType,
            deathInfo.CharacterType));

        CharacterDied?.Invoke(deathInfo);
    }
}