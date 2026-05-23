using UnityEngine;
using Zenject;

public class CharacterSpawnerInstaller : MonoBehaviour {
    [SerializeField] private CharacterSpawner _characterSpawner;

    private CharacterFactory _characterFactory;
    private CharacterPool _characterPool;

    [Inject]
    private void Consruct(
        CharacterFactory characterFactory,
        CharacterPool characterPool) {
        _characterFactory = characterFactory;
        _characterPool = characterPool;
    }

    private void Awake() {
        _characterSpawner.Initialize(_characterFactory, _characterPool);
    }
}
