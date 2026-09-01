using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConveyorWars.Units.Movement {
    [Serializable]
    public sealed class UnitMovementSettings {
        [SerializeField, MinValue(0.01f)] private float _moveSpeed = 5f;
        [SerializeField, MinValue(0f)] private float _rotationSpeed = 540f;
        [SerializeField, MinValue(0f)] private float _stoppingDistance = 0.05f;

        public float MoveSpeed => _moveSpeed;
        public float RotationSpeed => _rotationSpeed;
        public float StoppingDistance => _stoppingDistance;
    }
}