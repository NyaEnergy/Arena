public interface ICharacterRuntimeConfig {
    CharacterView Prefab { get; }
    CharacterCombatRow CombatRow { get; }

    CharacterPresencePresentationConfig EntryPresentation { get; }
    CharacterPresencePresentationConfig ExitPresentation { get; }

    float MaxHP { get; }
    float MoveSpeed { get; }
    float ThreatWeight { get; }
}