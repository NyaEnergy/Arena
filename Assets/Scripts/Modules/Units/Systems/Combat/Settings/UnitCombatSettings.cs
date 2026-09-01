using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace ConveyorWars.Units.Combat {
    [Serializable]
    public sealed class UnitCombatSettings {
        [SerializeField, MinValue(1)] private int _maxHealth = 100;
        [SerializeField, MinValue(1)] private int _damage = 10;
        [SerializeField, MinValue(0.01f)] private float _attackRange = 1.5f;
        [SerializeField, MinValue(0.01f)] private float _attackInterval = 1f;

        public int MaxHealth => _maxHealth;
        public int Damage => _damage;
        public float AttackRange => _attackRange;
        public float AttackInterval => _attackInterval;
    }
}