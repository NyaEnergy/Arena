using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Queue/Enemy Conveyor Config",
                 fileName = "EnemyConveyorConfig")]
public class EnemyConveyorConfig : ScriptableObject {
    [SerializeField, Min(0f)] private float _startDelay = 2f;
    [SerializeField, Min(0.1f)] private float _feedInterval = 4f;
    [SerializeField] private List<EnemyConveyorEntry> _entries = new();

    public float StartDelay => Mathf.Max(0f, _startDelay);
    public float FeedInterval => Mathf.Max(0.1f, _feedInterval);
    public IReadOnlyList<EnemyConveyorEntry> Entries => _entries;
}