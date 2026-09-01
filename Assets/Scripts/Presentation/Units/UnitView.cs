using ConveyorWars.Presentation.Combat;
using UnityEngine;

namespace ConveyorWars.Presentation.Units {
    public abstract class UnitView : MonoBehaviour {
        public abstract Transform Body { get; }
        public abstract HealthBarView HealthBar { get; }
    }
}
