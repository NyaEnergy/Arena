using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;

namespace ConveyorWars.Presentation.Combat {
    public sealed class HealthBarView : MonoBehaviour {
        [SerializeField, Required] private Image _fill;

        public Image Fill => _fill;
    }
}
