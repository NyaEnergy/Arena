using System;

public class CharacterDeathEventService {
    public event Action<CharacterDeathInfo> CharacterDied;

    public void NotifyDeath(CharacterDeathInfo deathInfo) {
        CharacterDied?.Invoke(deathInfo);
    }
}