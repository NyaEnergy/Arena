using R3;

public class TargetComponent {
    private readonly ReactiveProperty<CharacterBrain> _currentTarget = new(null);
    public ReadOnlyReactiveProperty<CharacterBrain> CurrentTarget => _currentTarget;

    public void SetTarget(CharacterBrain target) {
        _currentTarget.Value = target;
    }

    public void ClearTarget() {
        _currentTarget.Value = null;
    }
}
