using ConveyorWars.Units;
using UnityEngine;

namespace ConveyorWars.Application {
    public sealed class UnitDecisionProcessor {
        private readonly UnitRegistry _unitRegistry;
        private readonly ActiveAllyGroupRuntime _activeGroupRuntime;
        private readonly AllyFormationSettings _groupSettings;

        public UnitDecisionProcessor(UnitRegistry unitRegistry,
                                     ActiveAllyGroupRuntime activeGroupRuntime,
                                     AllyFormationSettings groupSettings) {
            _unitRegistry = unitRegistry;
            _activeGroupRuntime = activeGroupRuntime;
            _groupSettings = groupSettings;
        }

        public void Tick(UnitInstance unit) {
            if (_activeGroupRuntime.IsLeader(unit.Entity) ||
                !unit.Combatant.IsAlive ||
                unit.Combatant.HasTarget ||
                !unit.AIRuntime.IsDecisionReady) {
                return;
            }

            if (!unit.AIRuntime.TryStartDecisionCooldown(
                    unit.AISettings.DecisionInterval)) {
                return;
            }

            if (TryFindTarget(unit, out UnitInstance target)) {
                unit.Combatant.TrySetTarget(target.Entity);
            }
        }

        private bool TryFindTarget(UnitInstance unit,
                                   out UnitInstance target) {
            target = null;

            bool isAssistant = _activeGroupRuntime.IsAssistant(unit.Entity);

            UnitInstance leader = null;

            if (isAssistant && !TryGetAvailableLeader(out leader)) {
                return false;
            }

            if (isAssistant && !IsWithinDistance(unit.View.transform.position,
                                  leader.View.transform.position,
                                  _groupSettings.CombatLeashDistance)) {
                return false;
            }

            float nearestDistance = float.PositiveInfinity;

            for (int i = 0; i < _unitRegistry.Units.Count; i++) {
                UnitInstance candidate = _unitRegistry.Units[i];

                if (!candidate.Combatant.IsAlive ||
                    candidate.Entity.Side == unit.Entity.Side) {
                    continue;
                }

                if (isAssistant && !IsWithinDistance(candidate.View.transform.position,
                                      leader.View.transform.position,
                                      _groupSettings.CombatEngageDistance)) {
                    continue;
                }

                Vector3 offset = candidate.View.transform.position -
                                 unit.View.transform.position;

                offset.y = 0f;

                float distance = offset.sqrMagnitude;

                if (distance >= nearestDistance) continue;

                nearestDistance = distance;
                target = candidate;
            }

            return target != null;
        }

        private bool TryGetAvailableLeader(out UnitInstance leader) {
            leader = null;

            return _activeGroupRuntime.Leader != null &&
                   _unitRegistry.TryGet(_activeGroupRuntime.Leader,
                                        out leader) &&
                   leader.Combatant.IsAlive;
        }

        private bool IsWithinDistance(Vector3 first,
                                      Vector3 second,
                                      float distance) {
            Vector3 offset = first - second;
            offset.y = 0f;

            return offset.sqrMagnitude <= distance * distance;
        }
    }
}