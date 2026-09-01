using UnityEngine;

namespace ConveyorWars.Units {
    [System.Serializable]
    public sealed class AllyFormationSettings {
        [SerializeField] private float _slotDistance = 2.5f;
        [SerializeField] private float _forwardOffset = -2f;
        [SerializeField] private float _formationTolerance = 0.75f;

        [SerializeField] private float _combatEngageDistance = 8f;
        [SerializeField] private float _combatLeashDistance = 10f;

        public float SlotDistance => _slotDistance;
        public float ForwardOffset => _forwardOffset;
        public float FormationTolerance => _formationTolerance;

        public float CombatEngageDistance => _combatEngageDistance;
        public float CombatLeashDistance => _combatLeashDistance;
    }
}