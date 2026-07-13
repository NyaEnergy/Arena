using UnityEngine;

public readonly struct SummonerDeathInfo {
    public readonly SummonerPoolKey PoolKey;
    public readonly Vector3 Position;

    public TeamType TeamType => PoolKey.TeamType;
    public SummonerConfig Config => PoolKey.Config;

    public SummonerDeathInfo(SummonerPoolKey poolKey,
                             Vector3 position) {
        PoolKey = poolKey;
        Position = position;
    }
}