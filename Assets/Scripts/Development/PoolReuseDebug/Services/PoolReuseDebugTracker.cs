using System.Collections.Generic;

public class PoolReuseDebugTracker {
    private readonly HashSet<BattlefieldCharacter> _observedCharacters = new();

    public int UniqueCharacterCount => _observedCharacters.Count;

    public bool HasReusedCharacter { get; private set; }

    public void Register(BattlefieldCharacter character) {
        if (character == null) return;

        if (!_observedCharacters.Add(character))
            HasReusedCharacter = true;
    }
}
