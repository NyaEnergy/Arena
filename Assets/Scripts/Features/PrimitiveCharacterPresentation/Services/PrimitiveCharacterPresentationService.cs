public class PrimitiveCharacterPresentationService {
    private readonly PrimitiveCharacterPresentationConfig _config;

    public PrimitiveCharacterPresentationService(PrimitiveCharacterPresentationConfig config) {
        _config = config;
    }

    public void Apply(ICharacterConfig characterConfig,
                      TeamType teamType,
                      PrimitiveCharacterPresentationView view) {
        if (characterConfig == null) return;

        bool hasEntry = _config.TryGetEntry(characterConfig.CharacterType,
                                        out PrimitiveCharacterPresentationEntry entry);

        if (!hasEntry) return;

        bool hasMaterial = _config.TryGetMaterial(teamType, out UnityEngine.Material material);

        if (!hasMaterial) return;

        view.Apply(entry.PrimitiveType,
                   entry.LocalPosition,
                   entry.LocalScale,
                   material);
    }
}
