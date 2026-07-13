using UnityEngine;

public class MedicAIBehavior : ICharacterBehavior {
    private readonly CharacterBrain _brain;
    private readonly MedicTargetSelectionService _targetSelectionService;
    private readonly MedicPositioningService _positioningService;
    private readonly MedicHealingService _healingService;
    private readonly MedicCombatService _combatService;

    private readonly MedicHealingRuntime _healingRuntime = new();
    private readonly MedicCombatRuntime _combatRuntime = new();

    public MedicAIBehavior(CharacterBrain brain,
                           MedicTargetSelectionService targetSelectionService,
                           MedicPositioningService positioningService,
                           MedicHealingService healingService,
                           MedicCombatService combatService) {
        _brain = brain;
        _targetSelectionService = targetSelectionService;
        _positioningService = positioningService;
        _healingService = healingService;
        _combatService = combatService;
    }

    public void Reset() {
        _healingRuntime.Clear();
        _combatRuntime.Reset();

        ClearHealingPresentation();

        _brain.TargetComponent.ClearTarget();
        _brain.MovementComponent.Stop();
    }

    public void Tick() {
        _targetSelectionService.UpdateTarget(
            _brain, _healingRuntime);

        CharacterBrain healingTarget = _healingRuntime.Target;

        if (healingTarget != null) {
            SupportAlly(healingTarget, true);
            return;
        }

        CharacterBrain companion =
            _targetSelectionService.FindCompanion(_brain);

        if (companion != null) {
            SupportAlly(companion, false);
            return;
        }

        ClearHealingPresentation();

        _combatService.Tick(_brain, _combatRuntime);
    }

    private void SupportAlly(CharacterBrain ally,
                             bool shouldHeal) {

        _brain.TargetComponent.ClearTarget();

        Vector3 supportPosition =
            _positioningService.GetSupportPosition(_brain, ally);

        _brain.MovementComponent.MoveToPosition(supportPosition);

        if (!shouldHeal) {
            ClearHealingPresentation();
            return;
        }


        bool isHealing = _healingService.TryHeal(_brain, ally);
        
        UpdateHealingPresentation(ally, isHealing);
    }

    private void UpdateHealingPresentation(CharacterBrain ally,
                                           bool isHealing) {

        MedicView medicView = _brain.View as MedicView;

        if (medicView == null) return;

        if (!isHealing) {
            medicView.ClearHealingTarget();
            return;
        }

        medicView.SetHealingTarget(ally.View);
    }

    private void ClearHealingPresentation() {
        MedicView medicView = _brain.View as MedicView;
        medicView?.ClearHealingTarget();
    }
}