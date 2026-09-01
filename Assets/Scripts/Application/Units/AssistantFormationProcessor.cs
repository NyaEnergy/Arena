using ConveyorWars.Units;
using UnityEngine;

namespace ConveyorWars.Application {
    public sealed class AssistantFormationProcessor {
        private readonly ActiveAllyGroupRuntime _groupRuntime;
        private readonly UnitRegistry _unitRegistry;
        private readonly AllyFormationSettings _settings;

        public AssistantFormationProcessor(ActiveAllyGroupRuntime groupRuntime,
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
                unit.Combatant.HasTarget ||
                _groupRuntime.Leader == null ||
                !_unitRegistry.TryGet(_groupRuntime.Leader,
                                      out UnitInstance leader) ||
                !leader.Combatant.IsAlive) {
                return;
            }

            int slot = _groupRuntime.GetAssistantSlot(unit.Entity);

            if (slot < 0 || slot > 1) return;

            Vector3 destination = CalculateSlotPosition(leader, slot);

            Vector3 toSlot =
                destination - unit.View.transform.position;

            toSlot.y = 0f;

            float tolerance = _settings.FormationTolerance;

            if (toSlot.sqrMagnitude <= tolerance * tolerance) {
                if (unit.MovementMotor.State ==
                    ConveyorWars.Units.Movement.UnitMovementState.Moving) {
                    unit.MovementMotor.Stop();
                }

                return;
            }

            unit.MovementMotor.TrySetDestination(destination);
        }

        private Vector3 CalculateSlotPosition(UnitInstance leader, int slot) {
            float side = slot == 0 ? -1f : 1f;

            Vector3 localOffset = new(
                side * _settings.SlotDistance,
                0f,
                _settings.ForwardOffset);

            return leader.View.transform.position +
                   leader.View.transform.rotation * localOffset;
        }
    }
}