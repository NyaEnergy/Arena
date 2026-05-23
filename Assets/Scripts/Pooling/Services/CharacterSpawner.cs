using UnityEngine;

public class CharacterSpawner : MonoBehaviour {
    [SerializeField] private BattlefieldCharacter _allyPrefab;
    [SerializeField] private BattlefieldCharacter _enemyPrefab;

    [SerializeField] private Transform _allySpawnPoint;
    [SerializeField] private Transform _enemySpawnPoint;

    private CharacterFactory _characterFactory;
    private CharacterPool _characterPool;

    public void Initialize(
        CharacterFactory characterFactory,
        CharacterPool characterPool) {
        _characterFactory = characterFactory;
        _characterPool = characterPool;

        _characterPool.Initialize(CharacterType.Ally, _allyPrefab, 8);
        _characterPool.Initialize(CharacterType.Enemy, _enemyPrefab, 8);
    }

    private void Start() {
        _characterFactory.Spawn(CharacterType.Ally, _allySpawnPoint.position);
        _characterFactory.Spawn(CharacterType.Enemy, _enemySpawnPoint.position);
    }
}
