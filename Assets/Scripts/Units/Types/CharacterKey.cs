using System;

public readonly struct CharacterKey : IEquatable<CharacterKey> {
    public readonly TeamType TeamType;
    public readonly CharacterType CharacterType;

    public CharacterKey(TeamType teamType,
                        CharacterType characterType) {
        TeamType = teamType;
        CharacterType = characterType;
    }

    public bool Equals(CharacterKey other) {
        return TeamType == other.TeamType &&
            CharacterType == other.CharacterType;
    }

    public override bool Equals(object obj) {
        return obj is CharacterKey other &&
            Equals(other);
    }

    public override int GetHashCode() {
        return HashCode.Combine(TeamType, CharacterType);
    }

    public static bool operator ==(CharacterKey left, CharacterKey right) {
        return left.Equals(right);
    }

    public static bool operator !=(CharacterKey left, CharacterKey right) {
        return !left.Equals(right);
    }
}
