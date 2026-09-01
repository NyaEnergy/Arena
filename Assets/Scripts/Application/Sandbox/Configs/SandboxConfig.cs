using ConveyorWars.Units;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConveyorWars.Application {
    [CreateAssetMenu(fileName = "SandboxConfig",
                     menuName = "Conveyor Wars/Sandbox/Sandbox Config")]
    public sealed class SandboxConfig : ScriptableObject {
        [SerializeField, Required] private UnitConfig _ally;
        [SerializeField, Required] private UnitConfig _legionMelee;
        [SerializeField, Required] private UnitConfig _legionRanged;

        public UnitConfig Ally => _ally;
        public UnitConfig LegionMelee => _legionMelee;
        public UnitConfig LegionRanged => _legionRanged;
    }
}