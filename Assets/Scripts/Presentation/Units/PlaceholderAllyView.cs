using ConveyorWars.Presentation.Combat;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConveyorWars.Presentation.Units {
    public sealed class PlaceholderAllyView : UnitView {
        [SerializeField, Required] private Transform _body;
        [SerializeField, Required] private Transform _cameraTarget;
        [SerializeField, Required] private HealthBarView _healthBar;
        [SerializeField] private LineRenderer _leaderRing;

        public Transform CameraTarget => _cameraTarget;

        public override Transform Body => _body;
        public override HealthBarView HealthBar => _healthBar;
        public LineRenderer LeaderRing => _leaderRing;
    }
}
