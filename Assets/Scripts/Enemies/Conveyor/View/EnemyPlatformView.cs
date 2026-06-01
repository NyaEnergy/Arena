using UnityEngine;

public class EnemyPlatformView : MonoBehaviour {
    [SerializeField] private Transform _enemyAnchor;
    public Transform EnemyAnchor => _enemyAnchor;

    public void Enable() {
        gameObject.SetActive(true);
    }

    public void Disable() {
        gameObject.SetActive(false);
    }
}
