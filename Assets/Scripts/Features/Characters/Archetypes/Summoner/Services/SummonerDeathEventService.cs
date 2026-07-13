using System;

public class SummonerDeathEventService {
    public event Action<SummonerDeathInfo> SummonerDied;

    public void NotifyDeath(SummonerDeathInfo deathInfo) {
        SummonerDied?.Invoke(deathInfo);
    }
}