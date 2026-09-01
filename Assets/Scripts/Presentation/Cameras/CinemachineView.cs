using Sirenix.OdinInspector;
using Unity.Cinemachine;
using UnityEngine;

namespace ConveyorWars.Presentation.Cameras {
    public sealed class CinemachineView : MonoBehaviour {
        [SerializeField, Required] private CinemachineCamera _camera;
        [SerializeField] private Transform _trackingTarget;
        [SerializeField, Required] private CinemachineOrbitalFollow _orbitalFollow;

        public CinemachineCamera Camera => _camera;
        public Transform TrackingTarget => _trackingTarget;
        public CinemachineOrbitalFollow OrbitalFollow => _orbitalFollow;
    }
}