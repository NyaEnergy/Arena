using ConveyorWars.Units.AI;
using ConveyorWars.Units.Movement;
using UnityEngine;

namespace ConveyorWars.Application {
    public sealed class UnitStateMachine {
        private readonly UnitRegistry _unitRegistry;

        public UnitStateMachine(UnitRegistry unitRegistry) {
            _unitRegistry = unitRegistry;
        }

        public void Tick(UnitInstance unit) {
            if (!unit.Combatant.IsAlive) {
                EnterDead(unit);
                return;
            }

            if (!unit.Combatant.HasTarget) {
                UpdateWithoutTarget(unit);
                return;
            }

            if (!_unitRegistry.TryGet(unit.Combatant.CurrentTarget,
                                  out UnitInstance target) ||
                !target.Combatant.IsAlive) {
                unit.Combatant.ClearTarget();
                unit.MovementMotor.Stop();
                unit.StateRuntime.Set(UnitState.Idle);
                return;
            }

            UpdateCombat(unit, target);
        }

        private void UpdateWithoutTarget(UnitInstance unit) {
            UnitState state = unit.MovementMotor.State == UnitMovementState.Moving ?
                                UnitState.Move : UnitState.Idle;

            unit.StateRuntime.Set(state);
        }

        private void UpdateCombat(UnitInstance attacker, UnitInstance target) {
            Vector3 targetPosition = target.View.transform.position;

            Vector3 toTarget = targetPosition -
                               attacker.View.transform.position;

            toTarget.y = 0f;

            float range = attacker.Combatant.AttackRange;

            if (toTarget.sqrMagnitude > range * range) {
                attacker.StateRuntime.Set(UnitState.Approach);

                attacker.MovementMotor.TrySetDestination(targetPosition);

                return;
            }

            attacker.StateRuntime.Set(UnitState.Attack);

            attacker.MovementMotor.Stop();
            attacker.MovementMotor.TryFace(targetPosition);

            if (!attacker.MovementMotor.IsFacing(targetPosition)) return;

            TryAttack(attacker, target);
        }

        private void TryAttack(UnitInstance attacker,
                               UnitInstance target) {
            if (!attacker.Combatant.TryAttack(target.Combatant, out _)) return;

            attacker.AttackPresenter.PlayAttack(target.View.transform.position);

            target.AttackPresenter.PlayHit();

            if (!target.Combatant.IsAlive) {
                EnterDead(target);
            }
        }

        private void EnterDead(UnitInstance unit) {
            unit.StateRuntime.Set(UnitState.Dead);
            unit.MovementMotor.Stop();
            unit.DeathPresenter.Apply();
        }
    }
}