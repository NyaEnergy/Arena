public class HunterRuntime {
    public HunterCombatMode CombatMode;
    public float NextAttackTime;

    public void Reset() {
        CombatMode = HunterCombatMode.Ranged;
        NextAttackTime = float.NegativeInfinity;
    }
}
