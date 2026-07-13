using System;

public readonly struct SummonerPoolKey : IEquatable<SummonerPoolKey> {
    public readonly TeamType TeamType;
    public readonly SummonerConfig Config;

    public SummonerPoolKey(TeamType teamType,
                           SummonerConfig config) {
        TeamType = teamType;
        Config = config;
    }

    public bool Equals(SummonerPoolKey other) {
        return TeamType == other.TeamType &&
               Config == other.Config;
    }

    public override bool Equals(object obj) {
        return obj is SummonerPoolKey other &&
               Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(TeamType, Config);
    }
}