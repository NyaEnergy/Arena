using ConveyorWars.Presentation.Cameras;
using ConveyorWars.Presentation.Units;
using ConveyorWars.Units;
using UnityEngine;

namespace ConveyorWars.Application {
    public sealed class SandboxUnitInitializer {
        private readonly SandboxConfig _config;
        private readonly UnitFactory _unitFactory;
        private readonly ActiveAllyGroupRuntime _activeGroupRuntime;
        private readonly SandboxCameraController _cameraController;
        private readonly LeaderMarkerPresenter _leaderMarkerPresenter;

        public SandboxUnitInitializer(SandboxConfig config,
                                      UnitFactory unitFactory,
                                      ActiveAllyGroupRuntime activeGroupRuntime,
                                      SandboxCameraController cameraController,
                                      LeaderMarkerPresenter leaderMarkerPresenter) {
            _config = config;
            _unitFactory = unitFactory;
            _activeGroupRuntime = activeGroupRuntime;
            _cameraController = cameraController;
            _leaderMarkerPresenter = leaderMarkerPresenter;
        }

        public bool TryInitialize() {
            if (!TryInitializeAllies()) return false;

            if (!TryCreate(_config.LegionMelee,
                           new Vector3(5f, 0f, 4f),
                           out _)) {
                return false;
            }

            return TryCreate(_config.LegionRanged,
                             new Vector3(-5f, 0f, 5f),
                             out _);
        }

        private bool TryInitializeAllies() {
            if (!TryCreateActiveAlly(Vector3.zero, out UnitInstance leader) ||
                !TryInitializeLeaderPresentation(leader)) {
                return false;
            }

            if (!TryCreateActiveAlly(new Vector3(-1.5f, 0f, -1.5f), out _)) {
                return false;
            }

            if (!TryCreateActiveAlly(new Vector3(1.5f, 0f, -1.5f), out _)) {
                return false;
            }

            return _activeGroupRuntime.TryAddReserve(_config.Ally);
        }

        private bool TryCreateActiveAlly(Vector3 position, out UnitInstance unit) {
            if (!TryCreate(_config.Ally, position, out unit)) {
                return false;
            }

            return _activeGroupRuntime.TryAddActive(unit.Entity);
        }

        private bool TryInitializeLeaderPresentation(UnitInstance leader) {
            if (leader.View is not PlaceholderAllyView allyView ||
                !_cameraController.TryInitialize(allyView.CameraTarget)) {
                return false;
            }

            _leaderMarkerPresenter.SetLeader(allyView);
            return true;
        }

        private bool TryCreate(UnitConfig config,
                               Vector3 position,
                               out UnitInstance unit) {
            return _unitFactory.TryCreate(config,
                                          position,
                                          out unit);
        }
    }
}