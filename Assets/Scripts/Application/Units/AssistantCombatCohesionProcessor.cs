using ConveyorWars.Units;
using UnityEngine;

namespace ConveyorWars.Application {
    public sealed class AssistantCombatCohesionProcessor {
        private readonly ActiveAllyGroupRuntime _groupRuntime;
        private readonly UnitRegistry _unitRegistry;
        private readonly AllyFormationSettings _settings;

        public AssistantCombatCohesionProcessor(ActiveAllyGroupRuntime groupRuntime,
                                                UnitRegistry unitRegistry,
                                                AllyFormationSettings settings) {
            _groupRuntime = groupRuntime;
            _unitRegistry = unitRegistry;
            _settings = settings;
        }

        public void Tick(UnitInstance unit) {
            if (unit == null ||
                !unit.Combatant.IsAlive ||
                !_groupRuntime.IsAssistant(unit.Entity) ||
                !unit.Combatant.HasTarget ||
                _groupRuntime.Leader == null ||
                !_unitRegistry.TryGet(_groupRuntime.Leader,
                                      out UnitInstance leader) ||
                !leader.Combatant.IsAlive) {
                return;
            }

            if (!_unitRegistry.TryGet(unit.Combatant.CurrentTarget,
                                      out UnitInstance target) ||
                !target.Combatant.IsAlive) {
                return;
            }

            float leashDistance = _settings.CombatLeashDistance;

            if (IsWithinDistance(unit.View.transform.position,
                                 leader.View.transform.position,
                                 leashDistance) &&
                IsWithinDistance(target.View.transform.position,
                                 leader.View.transform.position,
                                 leashDistance)) {
                return;
            }

            unit.Combatant.ClearTarget();
            unit.MovementMotor.Stop();
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