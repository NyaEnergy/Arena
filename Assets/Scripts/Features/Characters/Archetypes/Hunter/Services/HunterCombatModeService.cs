public class HunterCombatModeService {
    private readonly HunterConfig _config;

    public HunterCombatModeService(HunterConfig config) {
        _config = config;
    }

    public void UpdateMode(HunterRuntime runtime,
                           float sqrDistanceToTarget) {
        Range switchRange = _config.MeleeModeSwitchRange;

        if (runtime.CombatMode == HunterCombatMode.Ranged) {

            float sqrEnterDistance = switchRange.Min *
                                     switchRange.Min;

            if (sqrDistanceToTarget <= sqrEnterDistance) {
                runtime.CombatMode = HunterCombatMode.Melee;
            }

            return;
        }

        float sqrExitDistance = switchRange.Max *
                                switchRange.Max;

        if (sqrDistanceToTarget >= sqrExitDistance) {
            runtime.CombatMode = HunterCombatMode.Ranged;
        }
    }
}