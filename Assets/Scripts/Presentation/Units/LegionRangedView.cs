using ConveyorWars.Presentation.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConveyorWars.Presentation.Units {
    public sealed class LegionRangedView : UnitView {
        [SerializeField, Required] private Transform _body;
        [SerializeField, Required] private HealthBarView _healthBar;

        public override Transform Body => _body;
        public override HealthBarView HealthBar => _healthBar;
    }
}