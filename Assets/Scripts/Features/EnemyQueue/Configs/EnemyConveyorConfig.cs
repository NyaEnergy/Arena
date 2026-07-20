using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Queue/Enemy Conveyor Config",
                 fileName = "EnemyConveyorConfig")]
public class EnemyConveyorConfig : ScriptableObject {
    [SerializeField, Min(0f)] private float _startDelay = 2f;

    public float StartDelay => Mathf.Max(0f, _startDelay);
}