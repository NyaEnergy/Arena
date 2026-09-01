using ConveyorWars.Presentation.Combat;
using ConveyorWars.Presentation.Units;
using ConveyorWars.Units;
using ConveyorWars.Units.AI;
using ConveyorWars.Units.Combat;
using ConveyorWars.Units.Movement;
using UnityEngine;

namespace ConveyorWars.Application {
    public sealed class UnitFactory {
        private readonly UnitRegistry _registry;
        private readonly Camera _camera;

        public UnitFactory(UnitRegistry registry,
                           Camera camera) {
            _registry = registry;
            _camera = camera;
        }

        public bool TryCreate(UnitConfig config,
                              Vector3 position,
                          out UnitInstance unit) {
            unit = null;

            if (config == null ||
                config.Prefab == null ||
                config.Movement == null ||
                config.Combat == null) {
                return false;
            }

            GameObject root =
                Object.Instantiate(
                    config.Prefab,
                    position,
                    Quaternion.identity);

            if (!root.TryGetComponent(out UnitView view) ||
                !root.TryGetComponent(out Collider unitCollider)) {
                    Object.Destroy(root);
                    return false;
            }

            UnitEntity entity = new UnitEntity(config.Side);

            UnitMovementRuntime movementRuntime = new UnitMovementRuntime();

            UnitMovementMotor movementMotor =
                new UnitMovementMotor(
                    view.transform,
                    unitCollider,
                    config.Movement,
                    movementRuntime);

            UnitCombatRuntime combatRuntime =
                new UnitCombatRuntime(
                    config.Combat.MaxHealth);

            UnitCombatant combatant =
                new UnitCombatant(
                    entity,
                    config.Combat,
                    combatRuntime);

            UnitStateRuntime stateRuntime =
                new UnitStateRuntime();

            UnitAIRuntime aiRuntime =
                new UnitAIRuntime();

            HealthBarPresenter healthBarPresenter =
                new HealthBarPresenter(
                    combatant,
                    view.HealthBar,
                    _camera);

            UnitAttackPresenter attackPresenter =
                new UnitAttackPresenter(view);

            UnitDeathPresenter deathPresenter =
                new UnitDeathPresenter(view);

            unit = new UnitInstance(
                entity,
                view,
                movementMotor,
                combatant,
                stateRuntime,
                aiRuntime,
                config.AI,
                healthBarPresenter,
                attackPresenter,
                deathPresenter);

            if (_registry.TryRegister(unit)) return true;

            Object.Destroy(root);
            unit = null;
            return false;
        }
    }
}