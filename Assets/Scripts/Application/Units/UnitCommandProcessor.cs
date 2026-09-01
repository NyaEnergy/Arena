using ConveyorWars.Presentation.Input;
using ConveyorWars.Presentation.Units;
using UnityEngine;

namespace ConveyorWars.Application {
    public sealed class UnitCommandProcessor {
        private readonly UnitRegistry _unitRegistry;

        public UnitCommandProcessor(UnitRegistry unitRegistry) {
            _unitRegistry = unitRegistry;
        }

        public void Handle(UnitInstance unit,
                           GameplayCommand command) {
            switch (command.Type) {
                case GameplayCommandType.Move:
                    HandleMove(unit, command.WorldPoint);
                    break;

                case GameplayCommandType.UnitInteraction:
                    HandleUnitInteraction(unit, command.UnitView);
                    break;
            }
        }

        private void HandleMove(UnitInstance unit,
                                Vector3 destination) {
            unit.Combatant.ClearTarget();
            unit.MovementMotor.TrySetDestination(destination);
        }

        private void HandleUnitInteraction(UnitInstance unit,
                                           UnitView targetView) {
            if (!_unitRegistry.TryGet(targetView, out UnitInstance target) ||
                !target.Combatant.IsAlive) {
                    return;
            }

            unit.Combatant.TrySetTarget(target.Entity);
        }
    }
}