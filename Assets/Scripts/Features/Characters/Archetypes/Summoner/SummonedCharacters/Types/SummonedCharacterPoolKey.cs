using System;

public readonly struct SummonedCharacterPoolKey : IEquatable<SummonedCharacterPoolKey> {
    public readonly TeamType TeamType;
    public readonly SummonedCharacterConfig Config;

    public SummonedCharacterPoolKey(TeamType teamType,
                                    SummonedCharacterConfig config) {
        TeamType = teamType;
        Config = config;
    }

    public bool Equals(
        SummonedCharacterPoolKey other) {
        return TeamType == other.TeamType &&
               Config == other.Config;
    }

    public override bool Equals(object obj) {
        return obj is SummonedCharacterPoolKey other &&
               Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(TeamType, Config);
    }
}