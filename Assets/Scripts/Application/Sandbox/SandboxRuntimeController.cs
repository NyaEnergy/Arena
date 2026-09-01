using System;
using UnityEngine;
using Zenject;
using ConveyorWars.Presentation.Cameras;
using ConveyorWars.Presentation.Input;
using ConveyorWars.Presentation.Units;
using ConveyorWars.Units;

namespace ConveyorWars.Application {
    public sealed class SandboxRuntimeController : IInitializable,
                                                   ITickable,
                                                   ILateTickable,
                                                   IDisposable {
        private readonly GameplayInputRuntime _inputRuntime;
        private readonly GameplayCommandInput _commandInput;
        private readonly GameplaySelectionInput _selectionInput;

        private readonly SandboxCameraController _cameraController;
        private readonly SandboxUnitInitializer _unitInitializer;
        private readonly SandboxActiveAllyGroupLifecycle _groupLifecycle;

        private readonly UnitCommandProcessor _commandProcessor;
        private readonly UnitRuntimeProcessor _runtimeProcessor;
        private readonly UnitRegistry _unitRegistry;
        private readonly ActiveAllyGroupRuntime _activeGroupRuntime;

        private readonly LeaderMarkerPresenter _leaderMarkerPresenter;

        private bool _isInitialized;

        public SandboxRuntimeController(GameplayInputRuntime inputRuntime,
                                        GameplayCommandInput commandInput,
                                        GameplaySelectionInput selectionInput,
                                        SandboxCameraController cameraController,
                                        SandboxUnitInitializer unitInitializer,
                                        SandboxActiveAllyGroupLifecycle groupLifecycle,
                                        UnitCommandProcessor commandProcessor,
                                        UnitRuntimeProcessor runtimeProcessor,
                                        UnitRegistry unitRegistry,
                                        ActiveAllyGroupRuntime activeGroupRuntime,
                                        LeaderMarkerPresenter leaderMarkerPresenter) {
            _inputRuntime = inputRuntime;
            _commandInput = commandInput;
            _selectionInput = selectionInput;
            _cameraController = cameraController;
            _unitInitializer = unitInitializer;
            _groupLifecycle = groupLifecycle;
            _commandProcessor = commandProcessor;
            _runtimeProcessor = runtimeProcessor;
            _unitRegistry = unitRegistry;
            _activeGroupRuntime = activeGroupRuntime;
            _leaderMarkerPresenter = leaderMarkerPresenter;
        }

        public void Initialize() {
            if (!_unitInitializer.TryInitialize()) return;
            _inputRuntime.Enable();
            _isInitialized = true;
        }

        public void Tick() {
            if (!_isInitialized) return;

            float deltaTime = Time.deltaTime;

            _cameraController.Tick(deltaTime);

            HandleLeaderSelection();
            HandleCommandInput();

            _runtimeProcessor.Tick(deltaTime);

            _groupLifecycle.Tick();
            _leaderMarkerPresenter.Tick();
        }

        public void LateTick() {
            if (!_isInitialized) return;
            _runtimeProcessor.LateTick();
        }

        public void Dispose() {
            _inputRuntime.Disable();
            _inputRuntime.Dispose();
        }

        private void HandleLeaderSelection() {
            if (!_selectionInput.TryReadUnit(out UnitView unitView) ||
                !_unitRegistry.TryGet(unitView, out UnitInstance newLeader) ||
                !newLeader.Combatant.IsAlive ||
                !_activeGroupRuntime.IsActive(newLeader.Entity) ||
                _activeGroupRuntime.IsLeader(newLeader.Entity) ||
                newLeader.View is not PlaceholderAllyView newLeaderView) {
                    return;
            }

            if (!_cameraController.TrySetTrackingTarget(newLeaderView.CameraTarget)) {
                return;
            }

            if (!_activeGroupRuntime.TrySetLeader(newLeader.Entity)) {
                return;
            }

            newLeader.Combatant.ClearTarget();
            newLeader.MovementMotor.Stop();

            _leaderMarkerPresenter.SetLeader(newLeaderView);
        }

        private void HandleCommandInput() {
            if (_activeGroupRuntime.Leader == null ||
                !_unitRegistry.TryGet(_activeGroupRuntime.Leader,
                                      out UnitInstance leader) ||
                !leader.Combatant.IsAlive ||
                !_commandInput.TryRead(out GameplayCommand command)) {
                return;
            }

            _commandProcessor.Handle(leader, command);
        }
    }
}