using UnityEngine;

public class CharacterBrain {
    private readonly CharacterView _view;
    private readonly CharacterConfig _config;
    private readonly CharacterRuntime _runtime;

    public CharacterView View => _view;
    public CharacterConfig Config => _config;
    public CharacterRuntime Runtime => _runtime;

    public CharacterBrain(CharacterView view, CharacterConfig config) {
        _view = view;
        _config = config;
        _runtime = new(config);
    }
}
