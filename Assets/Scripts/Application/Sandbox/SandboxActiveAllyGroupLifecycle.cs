using ConveyorWars.Presentation.Cameras;
using ConveyorWars.Presentation.Units;
using ConveyorWars.Units;
using UnityEngine;

namespace ConveyorWars.Application {
    public sealed class SandboxActiveAllyGroupLifecycle {
        private readonly ActiveAllyGroupRuntime _groupRuntime;
        private readonly UnitRegistry _unitRegistry;
        private readonly UnitFactory _unitFactory;
        private readonly AllyFormationSettings _formationSettings;
        private readonly SandboxCameraController _cameraController;
        private readonly LeaderMarkerPresenter _leaderMarkerPresenter;

        private Vector3 _reserveFallbackPosition;
        private bool _hasReserveFallbackPosition;

        public SandboxActiveAllyGroupLifecycle(ActiveAllyGroupRuntime groupRuntime,
                                               UnitRegistry unitRegistry,
                                               UnitFactory unitFactory,
                                               AllyFormationSettings formationSettings,
                                               SandboxCameraController cameraController,
                                               LeaderMarkerPresenter leaderMarkerPresenter) {
            _groupRuntime = groupRuntime;
            _unitRegistry = unitRegistry;
            _unitFactory = unitFactory;
            _formationSettings = formationSettings;
            _cameraController = cameraController;
            _leaderMarkerPresenter = leaderMarkerPresenter;
        }

        public void Tick() {
            UnitEntity previousLeader = _groupRuntime.Leader;

            _hasReserveFallbackPosition = false;

            RemoveUnavailableActiveUnits();

            if (_groupRuntime.Leader == null) {
                TryAssignLeader();
            }

            TryFillActiveFromReserve();

            if (_groupRuntime.Leader == null) {
                TryAssignLeader();
            }

            if (previousLeader != _groupRuntime.Leader) {
                ApplyLeaderPresentation();
            }
        }

        private void RemoveUnavailableActiveUnits() {
            for (int i = _groupRuntime.Active.Count - 1; i >= 0; i--) {
                UnitEntity entity = _groupRuntime.Active[i];

                if (_unitRegistry.TryGet(entity, out UnitInstance unit) &&
                    unit.Combatant.IsAlive) {
                    continue;
                }

                bool wasLeader = _groupRuntime.IsLeader(entity);

                if (_unitRegistry.TryGet(entity, out unit)) {
                    RememberVacantPosition(
                        unit.View.transform.position,
                        wasLeader);
                }

                _groupRuntime.TryRemoveActive(entity);
            }
        }

        private void RememberVacantPosition(Vector3 position, bool wasLeader) {
            if (_hasReserveFallbackPosition &&
                !wasLeader) {
                return;
            }

            _reserveFallbackPosition = position;
            _hasReserveFallbackPosition = true;
        }

        private void TryAssignLeader() {
            for (int i = 0; i < _groupRuntime.Active.Count; i++) {
                UnitEntity candidate = _groupRuntime.Active[i];

                if (!_unitRegistry.TryGet(candidate, out UnitInstance unit) ||
                    !unit.Combatant.IsAlive) {
                    continue;
                }

                if (_groupRuntime.TrySetLeader(candidate)) {
                    return;
                }
            }
        }

        private void TryFillActiveFromReserve() {
            while (_groupRuntime.HasFreeActiveSlot &&
                   _groupRuntime.TryPeekReserve(out UnitConfig reserveConfig)) {
                Vector3 spawnPosition = GetReserveSpawnPosition();

                if (!_unitFactory.TryCreate(reserveConfig,
                                            spawnPosition,
                                            out UnitInstance reserveUnit)) {
                    return;
                }

                if (!_groupRuntime.TryAddActive(reserveUnit.Entity)) {
                    return;
                }

                _groupRuntime.TryTakeReserve(out _);
            }
        }

        private Vector3 GetReserveSpawnPosition() {
            if (TryGetLivingLeader(out UnitInstance leader)) {
                int assistantSlot = _groupRuntime.ActiveCount - 1;

                float side = assistantSlot == 0 ?
                             -1f : 1f;

                Vector3 localOffset = new(
                    side * _formationSettings.SlotDistance,
                    0f,
                    _formationSettings.ForwardOffset);

                return leader.View.transform.position +
                       leader.View.transform.rotation * localOffset;
            }

            if (_hasReserveFallbackPosition) {
                return _reserveFallbackPosition;
            }

            return Vector3.zero;
        }

        private bool TryGetLivingLeader(out UnitInstance leader) {
            leader = null;

            return _groupRuntime.Leader != null &&
                   _unitRegistry.TryGet(_groupRuntime.Leader, out leader) &&
                   leader.Combatant.IsAlive;
        }

        private void ApplyLeaderPresentation() {
            if (!TryGetLivingLeader(out UnitInstance leader) ||
                leader.View is not PlaceholderAllyView leaderView) {
                _leaderMarkerPresenter.SetLeader(null);
                return;
            }

            leader.Combatant.ClearTarget();
            leader.MovementMotor.Stop();

            _cameraController.TrySetTrackingTarget(
                leaderView.CameraTarget);

            _leaderMarkerPresenter.SetLeader(leaderView);
        }
    }
}