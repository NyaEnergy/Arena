using UnityEngine;

public class CharacterInstaller : MonoBehaviour {
    [SerializeField] private CharacterView _view;
    [SerializeField] private CharacterConfig _config;

    private CharacterBrain _brain;

    public CharacterBrain Brain => _brain;

    private void Awake() {
        _brain = new CharacterBrain(_view, _config);
    }
}
