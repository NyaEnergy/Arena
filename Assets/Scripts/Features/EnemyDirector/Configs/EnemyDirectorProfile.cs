using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyDirectorProfile {
    [SerializeField, Min(0.1f)] private float _feedInterval = 4f;
    [SerializeField] private List<EnemyConveyorEntry> _entries = new();

    public float FeedInterval => Mathf.Max(0.1f, _feedInterval);
    public IReadOnlyList<EnemyConveyorEntry> Entries => _entries;
}