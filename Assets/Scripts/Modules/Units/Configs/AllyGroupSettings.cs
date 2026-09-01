using UnityEngine;

namespace ConveyorWars.Units {
    [System.Serializable]
    public sealed class AllyGroupSettings {
        [SerializeField] private float _followDistance = 2.5f;
        [SerializeField] private float _followTolerance = 0.5f;

        public float FollowDistance => _followDistance;
        public float FollowTolerance => _followTolerance;
    }
}