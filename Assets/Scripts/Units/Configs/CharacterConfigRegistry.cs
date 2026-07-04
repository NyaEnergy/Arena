using System.Collections.Generic;

public class CharacterConfigRegistry {
    private readonly Dictionary<CharacterType, ICharacterConfig> _configs = new();

    public CharacterConfigRegistry(List<ICharacterConfig> configs) {
        for (int i = 0; i < configs.Count; i++) {
            ICharacterConfig config = configs[i];

            if (config == null) continue;

            _configs[config.CharacterType] = config;
        }
    }

    public ICharacterConfig Get(CharacterType characterType) {
        _configs.TryGetValue(characterType,
            out ICharacterConfig config);

        return config;
    }

    public ICharacterConfig Get(CharacterKey key) {
        return Get(key.CharacterType);
    }
}