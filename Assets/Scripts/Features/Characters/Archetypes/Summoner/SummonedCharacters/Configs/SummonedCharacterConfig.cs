using UnityEngine;

public abstract class SummonedCharacterConfig : ScriptableObject,
                                                ICharacterRuntimeConfig {
    public abstract SummonedCharacterType SummonedCharacterType { get; }

    public abstract CharacterView Prefab { get; }
    public abstract CharacterCombatRow CombatRow { get; }

    public abstract CharacterPresencePresentationConfig EntryPresentation { get; }
    public abstract CharacterPresencePresentationConfig ExitPresentation { get; }

    public abstract float MaxHP { get; }
    public abstract float MoveSpeed { get; }
}