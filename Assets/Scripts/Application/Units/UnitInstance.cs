using ConveyorWars.Presentation.Combat;
using ConveyorWars.Presentation.Units;
using ConveyorWars.Units;
using ConveyorWars.Units.Combat;
using ConveyorWars.Units.Movement;
using ConveyorWars.Units.AI;

namespace ConveyorWars.Application {
    public sealed class UnitInstance {
        public UnitEntity Entity { get; }
        public UnitView View { get; }
        public UnitMovementMotor MovementMotor { get; }
        public UnitCombatant Combatant { get; }
        public UnitStateRuntime StateRuntime { get; }
        public UnitAIRuntime AIRuntime { get; }
        public UnitAISettings AISettings { get; }
        public HealthBarPresenter HealthBarPresenter { get; }
        public UnitAttackPresenter AttackPresenter { get; }
        public UnitDeathPresenter DeathPresenter { get; }

        public UnitInstance(UnitEntity entity,
                            UnitView view,
                            UnitMovementMotor movementMotor,
                            UnitCombatant combatant,
                            UnitStateRuntime stateRuntime,
                            UnitAIRuntime aiRuntime,
                            UnitAISettings aiSettings,
                            HealthBarPresenter healthBarPresenter,
                            UnitAttackPresenter attackPresenter,
                            UnitDeathPresenter deathPresenter) {
            Entity = entity;
            View = view;
            MovementMotor = movementMotor;
            Combatant = combatant;
            StateRuntime = stateRuntime;
            AIRuntime = aiRuntime;
            AISettings = aiSettings;
            HealthBarPresenter = healthBarPresenter;
            AttackPresenter = attackPresenter;
            DeathPresenter = deathPresenter;
        }
    }
}