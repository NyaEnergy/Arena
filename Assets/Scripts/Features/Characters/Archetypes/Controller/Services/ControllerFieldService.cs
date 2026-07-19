using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ControllerFieldService : ITickable {

    private readonly BattlefieldRegistry _registry;
    private readonly ControllerSlowService _slowService;
    private readonly ControllerFieldPool _fieldPool;

    private readonly List<ControllerFieldRuntime> _fields = new();

    public ControllerFieldService(BattlefieldRegistry registry,
                                  ControllerSlowService slowService,
                                  ControllerFieldPool fieldPool) {
        _registry = registry;
        _slowService = slowService;
        _fieldPool = fieldPool;
    }

    public bool Cast(CharacterBrain owner,
                     Vector3 position,
                     ControllerConfig config) {

        if (owner?.Runtime == null ||
            config == null) {
                return false;
        }

        float radius = Mathf.Max(0f, config.FieldRadius);
        float duration = Mathf.Max(0f, config.FieldDuration);

        if (radius <= 0f ||
            duration <= 0f) {
                return false;
        }

        TeamType teamType =
            owner.Runtime.TeamType;

        ControllerFieldView view = _fieldPool.Get(
                position, radius, config.GetFieldColor(teamType));

        _fields.Add(new ControllerFieldRuntime(
                teamType, position, radius,
                Mathf.Clamp01( config.SlowMultiplier),
                duration, view));

        return true;
    }

    public void Tick() {
        for (int i = _fields.Count - 1; i >= 0; i--) {

            ControllerFieldRuntime field = _fields[i];

            if (!field.Tick(Time.deltaTime)) {
                _fieldPool.Return(field.View);
                _fields.RemoveAt(i);
                    continue;
            }

            Apply(field);
        }
    }

    private void Apply(ControllerFieldRuntime field) {

        IReadOnlyList<CharacterBrain> targets =
            _registry.GetEnemies(field.TeamType);

        float sqrRadius = field.Radius * field.Radius;

        for (int i = 0; i < targets.Count; i++) {

            CharacterBrain target = targets[i];

            if (!IsAvailable(target)) continue;

            Vector3 difference = target.View.transform.position -
                                 field.Position;

            difference.y = 0f;

            if (difference.sqrMagnitude > sqrRadius) continue;

            _slowService.Apply(
                target, field.SlowMultiplier);
        }
    }

    private bool IsAvailable(CharacterBrain target) {

        return target != null &&
               target.View != null &&
               target.Runtime != null &&
               target.View.gameObject.activeInHierarchy &&
               !target.Runtime.IsDead.CurrentValue;
    }
}