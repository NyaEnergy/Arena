using UnityEngine;

public interface ICharacterConfig {
    CharacterType CharacterType { get; }

    float MaxHP { get; }
    float MoveSpeed { get; }

    Color HealthBarBackgroundColor { get; }
    Color HealthBarFillColor { get; }
}