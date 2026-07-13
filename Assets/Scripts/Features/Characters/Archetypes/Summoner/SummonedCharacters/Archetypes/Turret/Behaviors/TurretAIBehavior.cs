public class TurretAIBehavior : ICharacterBehavior {
    private readonly CharacterBrain _brain;
    private readonly TurretConfig _config;

    private readonly TurretTargetQueryService _targetQueryService;
    private readonly TurretShotService _shotService;
    private readonly TurretShotRuntime _shotRuntime = new();

    public TurretAIBehavior(CharacterBrain brain,
                            TurretConfig config,
                            TurretTargetQueryService targetQueryService,
                            TurretShotService shotService) {
        _brain = brain;
        _config = config;
        _targetQueryService = targetQueryService;
        _shotService = shotService;

        Reset();
    }

    public void Reset() {
        _brain.TargetComponent.ClearTarget();
        _brain.MovementComponent.Stop();
        _shotService.Reset(_shotRuntime);
    }

    public void Tick() {
        _shotService.Tick(_shotRuntime);
        _brain.MovementComponent.Stop();

        CharacterBrain target =
            _targetQueryService.FindClosest(
                _brain, _config);

        _brain.TargetComponent.SetTarget(target);

        if (target == null) return;

        if (_brain.View is not TurretView view)
            return;

        view.RotateTo(target.View);

        if (!_brain.CombatComponent.TryAttack())
            return;

        view.PlayAttack(target.View);
        _shotService.Play( view, target.View, _shotRuntime);
        target.View.PlayHit();
    }
}