using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConveyorWars.Units.AI {
    [Serializable]
    public sealed class UnitAISettings {
        [SerializeField, MinValue(0.05f)]
        private float _decisionInterval = 0.2f;

        public float DecisionInterval => _decisionInterval;
    }
}