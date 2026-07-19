using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ControllerSlowService : ILateTickable {
    private readonly Dictionary<CharacterBrain, float> _requests = new();

    private readonly HashSet<CharacterBrain> _affected = new();

    public void Apply(CharacterBrain target,
                      float multiplier) {
        if (!IsAvailable(target)) return;

        float value = Mathf.Clamp01(multiplier);

        if (_requests.TryGetValue(
                target, out float current)) {

            _requests[target] =
                Mathf.Min(current, value);

            return;
        }

        _requests.Add(target, value);
    }

    public void LateTick() {
        foreach (CharacterBrain target in _affected) {

            target?.MovementComponent
                .SetEffectSpeedMultiplier(1f);
        }

        _affected.Clear();

        foreach (KeyValuePair<CharacterBrain, float> pair in _requests) {

            if (!IsAvailable(pair.Key))
                continue;

            pair.Key.MovementComponent
                .SetEffectSpeedMultiplier(pair.Value);

            _affected.Add(pair.Key);
        }

        _requests.Clear();
    }

    private bool IsAvailable(CharacterBrain target) {

        return target != null &&
               target.View != null &&
               target.Runtime != null &&
               target.View.gameObject.activeInHierarchy &&
               !target.Runtime.IsDead.CurrentValue;
    }
}