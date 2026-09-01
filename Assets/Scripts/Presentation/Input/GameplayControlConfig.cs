using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;

namespace ConveyorWars.Presentation.Input {
    [CreateAssetMenu(
        fileName = "GameplayControlConfig",
        menuName = "Conveyor Wars/Input/Gameplay Control Config")]
    public sealed class GameplayControlConfig : ScriptableObject {
        [SerializeField] private LayerMask _groundMask;
        [SerializeField] private LayerMask _unitMask;

        [SerializeField, FormerlySerializedAs("_maxGroundRayDistance"), MinValue(0.1f)]
        private float _maxPointerRayDistance = 500f;

        [SerializeField, MinValue(0.01f)] private float _cameraRetargetSmoothTime = 0.25f;
        [SerializeField, MinValue(0.01f)] private float _cameraRotateSensitivity = 0.18f;
        [SerializeField, MinValue(0.01f)] private float _cameraRotateSmoothTime = 0.08f;
        [SerializeField, MinValue(0.01f)] private float _cameraZoomSensitivity = 0.12f;
        [SerializeField, MinValue(0.01f)] private float _cameraZoomSmoothTime = 0.12f;

        public LayerMask GroundMask => _groundMask;
        public LayerMask UnitMask => _unitMask;
        public float MaxPointerRayDistance => _maxPointerRayDistance;
        public float CameraRetargetSmoothTime => _cameraRetargetSmoothTime;
        public float CameraRotateSensitivity => _cameraRotateSensitivity;
        public float CameraRotateSmoothTime => _cameraRotateSmoothTime;
        public float CameraZoomSensitivity => _cameraZoomSensitivity;
        public float CameraZoomSmoothTime => _cameraZoomSmoothTime;
    }
}