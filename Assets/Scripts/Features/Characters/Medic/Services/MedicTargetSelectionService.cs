public class MedicTargetSelectionService {
    private const float FULL_HEALTH_THRESHOLD = 0.9999f;

    private readonly MedicConfig _config;
    private readonly MedicHealthService _healthService;
    private readonly MedicAllyQueryService _allyQueryService;
    private readonly MedicEmergencySwitchService _emergencySwitchService;

    public MedicTargetSelectionService(MedicConfig config,
                                       MedicHealthService healthService,
                                       MedicAllyQueryService allyQueryService,
                                       MedicEmergencySwitchService emergencySwitchService) {

        _config = config;
        _healthService = healthService;
        _allyQueryService = allyQueryService;
        _emergencySwitchService = emergencySwitchService;
    }

    public void UpdateTarget(CharacterBrain medic,
                             MedicHealingRuntime runtime) {
        ValidateCurrentTarget(medic, runtime);

        CharacterBrain emergencyTarget = FindEmergencyTarget(medic);

        if (TryKeepOrReplaceEmergencyTarget(runtime, emergencyTarget)) return;

        if (emergencyTarget != null) {
            runtime.SetTarget(emergencyTarget, MedicHealingMode.Emergency);
            return;
        }

        if (TryKeepCriticalTarget(runtime)) return;

        CharacterBrain criticalTarget = FindCriticalTarget(medic);

        if (criticalTarget != null) {
            runtime.SetTarget(criticalTarget, MedicHealingMode.Critical);
            return;
        }

        if (TryKeepFullRecoveryTarget(medic, runtime)) return;

        CharacterBrain woundedTarget =
            _allyQueryService.FindMostWounded(medic, FULL_HEALTH_THRESHOLD);

        if (woundedTarget == null) {
            runtime.Clear();
            return;
        }

        runtime.SetTarget(woundedTarget, MedicHealingMode.FullRecovery);
    }

    public CharacterBrain FindCompanion(CharacterBrain medic) {
        return _allyQueryService.FindClosestLiving(medic);
    }

    private void ValidateCurrentTarget(CharacterBrain medic, MedicHealingRuntime runtime) {
        if (_allyQueryService.IsHealingTarget(medic, runtime.Target)) {
            return;
        }

        runtime.Clear();
    }

    private CharacterBrain FindEmergencyTarget(CharacterBrain medic) {
        return _allyQueryService.FindMostWounded(medic,
            _config.EmergencyHealthRange.Min);
    }

    private CharacterBrain FindCriticalTarget(CharacterBrain medic) {
        return _allyQueryService.FindMostWounded(medic,
            _config.CriticalHealthRange.Min);
    }

    private bool TryKeepOrReplaceEmergencyTarget(MedicHealingRuntime runtime,
                                                 CharacterBrain emergencyTarget) {
        CharacterBrain currentTarget = runtime.Target;

        if (currentTarget == null ||
            runtime.Mode != MedicHealingMode.Emergency) {
            return false;
        }

        if (_emergencySwitchService.ShouldSwitch(
                currentTarget,
                emergencyTarget)) {

            runtime.SetTarget(emergencyTarget,
                MedicHealingMode.Emergency);

            return true;
        }

        float healthPercent =
            _healthService.GetHealthPercent(
                currentTarget);

        return healthPercent <
               _config.EmergencyHealthRange.Max;
    }

    private bool TryKeepCriticalTarget(MedicHealingRuntime runtime) {
        CharacterBrain currentTarget =runtime.Target;

        if (currentTarget == null ||
            runtime.Mode != MedicHealingMode.Critical) {

            return false;
        }

        float healthPercent =
            _healthService.GetHealthPercent(
                currentTarget);

        return healthPercent < _config.CriticalHealthRange.Max;
    }

    private bool TryKeepFullRecoveryTarget(CharacterBrain medic, MedicHealingRuntime runtime) {
        CharacterBrain currentTarget = runtime.Target;

        if (currentTarget == null ||
            runtime.Mode != MedicHealingMode.FullRecovery) {

            return false;
        }

        return _allyQueryService.IsHealingTarget(medic, currentTarget);
    }
}