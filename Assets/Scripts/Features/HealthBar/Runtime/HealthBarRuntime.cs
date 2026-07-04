using System;
using R3;
using UnityEngine;

public class HealthBarRuntime : IDisposable {
    private readonly HealthBarView _view;
    private readonly float _maxHP;
    private readonly IDisposable _currentHPSubscription;

    public HealthBarRuntime(CharacterBrain brain,
                            HealthBarView view,
                            Transform cameraTransform) {
        _view = view;
        _maxHP = brain.Config.MaxHP;

        _view.Initialize(cameraTransform,
                         brain.Config.HealthBarBackgroundColor,
                         brain.Config.HealthBarFillColor);

        UpdateView(brain.HealthComponent.CurrentHP.CurrentValue);
        _currentHPSubscription = brain.HealthComponent.CurrentHP.Subscribe(UpdateView);
    }

    public void Dispose() {
        _currentHPSubscription.Dispose();
    }

    private void UpdateView(float currentHP) {
        if(_maxHP <= 0f) {
            _view.SetNormalizedValue(0f);
            return;
        }
        _view.SetNormalizedValue(Mathf.Clamp01(currentHP / _maxHP));
    }
}
