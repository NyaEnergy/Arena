using ConveyorWars.Units.Combat;
using ConveyorWars.Units.Movement;
using Sirenix.OdinInspector;
using ConveyorWars.Units.AI;
using UnityEngine;

namespace ConveyorWars.Units {
    [CreateAssetMenu(fileName = "UnitConfig",
                     menuName = "Conveyor Wars/Units/Unit Config")]
    public sealed class UnitConfig : ScriptableObject {
        [SerializeField, Required, AssetsOnly] private GameObject _prefab;
        [SerializeField] private UnitSide _side;
        [SerializeField] private UnitMovementSettings _movement = new UnitMovementSettings();
        [SerializeField] private UnitCombatSettings _combat = new UnitCombatSettings();
        [SerializeField] private UnitAISettings _ai = new UnitAISettings();

        public GameObject Prefab => _prefab;
        public UnitSide Side => _side;
        public UnitMovementSettings Movement => _movement;
        public UnitCombatSettings Combat => _combat;
        public UnitAISettings AI => _ai;
    }
}