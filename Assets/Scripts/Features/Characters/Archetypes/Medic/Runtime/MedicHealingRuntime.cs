public class MedicHealingRuntime {
    private CharacterBrain _target;
    private MedicHealingMode _mode;

    public CharacterBrain Target => _target;
    public MedicHealingMode Mode => _mode;

    public void SetTarget(CharacterBrain target,
                          MedicHealingMode mode) {
        _target = target;
        _mode = mode;
    }

    public void Clear() {
        _target = null;
        _mode = MedicHealingMode.None;
    }
}
