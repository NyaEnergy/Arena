namespace ConveyorWars.Application {
    public sealed class UnitRuntimeProcessor {
        private readonly UnitRegistry _unitRegistry;
        private readonly UnitDecisionProcessor _decisionProcessor;
        private readonly AssistantCombatCohesionProcessor _cohesionProcessor;
        private readonly UnitStateMachine _stateMachine;
        private readonly AssistantFormationProcessor _formationProcessor;

        public UnitRuntimeProcessor(UnitRegistry unitRegistry,
                                    UnitDecisionProcessor decisionProcessor,
                                    AssistantCombatCohesionProcessor cohesionProcessor,
                                    UnitStateMachine stateMachine,
                                    AssistantFormationProcessor formationProcessor) {
            _unitRegistry = unitRegistry;
            _decisionProcessor = decisionProcessor;
            _cohesionProcessor = cohesionProcessor;
            _stateMachine = stateMachine;
            _formationProcessor = formationProcessor;
        }

        public void Tick(float deltaTime) {
            for (int i = 0; i < _unitRegistry.Units.Count; i++) {
                TickUnit(_unitRegistry.Units[i], deltaTime);
            }
        }

        public void LateTick() {
            for (int i = 0; i < _unitRegistry.Units.Count; i++) {
                UnitInstance unit = _unitRegistry.Units[i];

                if (unit.Combatant.IsAlive) {
                    unit.HealthBarPresenter.LateTick();
                }
            }
        }

        private void TickUnit(UnitInstance unit, float deltaTime) {
            if (unit == null ||
                !unit.Combatant.IsAlive) {
                return;
            }

            unit.AIRuntime.Tick(deltaTime);

            _decisionProcessor.Tick(unit);
            _cohesionProcessor.Tick(unit);
            _stateMachine.Tick(unit);
            _formationProcessor.Tick(unit);

            if (!unit.Combatant.IsAlive) return;

            unit.MovementMotor.Tick(deltaTime);
            unit.Combatant.Tick(deltaTime);
        }
    }
}