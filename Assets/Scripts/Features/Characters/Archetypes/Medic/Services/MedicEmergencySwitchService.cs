public class MedicEmergencySwitchService {
    private readonly MedicConfig _config;
    private readonly MedicHealthService _healthService;

    public MedicEmergencySwitchService(MedicConfig config,
                                       MedicHealthService healthService) {
        _config = config;
        _healthService = healthService;
    }

    public bool ShouldSwitch(CharacterBrain currentTarget,
                             CharacterBrain emergencyTarget) {
        if (currentTarget == null ||
           emergencyTarget == null ||
           currentTarget == emergencyTarget) return false;

        float currentHealthPercent = _healthService.GetHealthPercent(currentTarget);
        float emergencyHealthPercent = _healthService.GetHealthPercent(emergencyTarget);

        return emergencyHealthPercent + _config.EmergencySwitchDelta <
               currentHealthPercent;
    }
}
