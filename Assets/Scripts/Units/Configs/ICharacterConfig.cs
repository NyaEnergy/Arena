using UnityEngine;

public interface ICharacterConfig {
    CharacterType CharacterType { get; }
    CharacterView Prefab { get; }
    CharacterCombatRow CombatRow { get; }
    CharacterPresencePresentationSettings SpawnPresentation { get; }

    float MaxHP { get; }
    float MoveSpeed { get; }

    Color HealthBarBackgroundColor { get; }
    Color HealthBarFillColor { get; }
}