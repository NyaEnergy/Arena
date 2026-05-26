using System.Collections.Generic;

public class CharacterConfigRegistry {
    private readonly Dictionary<CharacterKey, CharacterConfig> _configs;

    public CharacterConfigRegistry(List<CharacterConfig> configs) {
        _configs = new Dictionary<CharacterKey, CharacterConfig>();

        foreach (CharacterConfig config in configs) {
            CharacterKey key =
                new CharacterKey(
                    config.TeamType,
                    config.CharacterType);

            _configs.Add(key, config);
        }
    }

    public CharacterConfig Get(CharacterKey key) {
        return _configs[key];
    }
}
