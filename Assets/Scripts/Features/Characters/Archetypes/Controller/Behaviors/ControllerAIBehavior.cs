using UnityEngine;

public class ControllerAIBehavior : ICharacterBehavior {

    private readonly CharacterBrain _brain;
    private readonly ControllerConfig _config;
    private readonly DetectionService _detectionService;
    private readonly ControllerPositioningService _positioningService;
    private readonly ControllerFieldService _fieldService;
    private readonly ControllerRuntime _runtime = new();

    public ControllerAIBehavior(CharacterBrain brain,
                                ControllerConfig config,
                                DetectionService detectionService,
                                ControllerPositioningService positioningService,
                                ControllerFieldService fieldService) {
        _brain = brain;
        _config = config;
        _detectionService = detectionService;
        _positioningService = positioningService;
        _fieldService = fieldService;

        Reset();
    }

    public void Reset() {
        _runtime.Reset();

        _brain.TargetComponent
              .ClearTarget();

        _brain.MovementComponent.Stop();
    }

    public void Tick() {
        CharacterBrain target = _detectionService
                                .FindClosestTarget(_brain);

        _brain.TargetComponent.SetTarget(target);

        if (target == null) {
            _brain.MovementComponent.Stop();
                return;
        }

        if (!_positioningService.Tick(
                _brain, target)) {
                    return;
        }

        if (!_runtime.IsReady(Time.time)) return;

        if (!_fieldService.Cast(_brain,
                                target.View
                                      .transform
                                      .position,
                                _config)) {
            return;
        }

        _runtime.StartCooldown(Time.time, _config.FieldCooldown);
        _brain.View.PlayAttack(target.View);
    }
}