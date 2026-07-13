using System;
using R3;
using UnityEngine;

public class HealthBarRuntime : IDisposable {
    private readonly HealthBarView _view;
    private readonly float _maxHP;

    private readonly IDisposable
        _currentHPSubscription;

    public HealthBarRuntime(CharacterBrain brain,
                            HealthBarView view,
                            Transform cameraTransform,
                            HealthBarPaletteConfig palette) {
        _view = view;
        _maxHP = brain.Config.MaxHP;

        TeamType teamType = brain.Runtime.TeamType;

        Color backgroundColor = palette != null ?
            palette.GetBackground(teamType) :
            Color.black;

        Color fillColor = palette != null ?
            palette.GetFill(teamType) : Color.white;

        _view.Initialize(cameraTransform,
                         backgroundColor,
                         fillColor);

        UpdateView(brain.HealthComponent
                        .CurrentHP
                        .CurrentValue);

        _currentHPSubscription = brain.HealthComponent
                                      .CurrentHP
                                      .Subscribe(UpdateView);
    }

    public void Dispose() {
        _currentHPSubscription?.Dispose();
    }

    private void UpdateView(float currentHP) {
        if (_maxHP <= 0f) {
            _view.SetNormalizedValue(0f);
            return;
        }

        _view.SetNormalizedValue(Mathf.Clamp01(currentHP / _maxHP));
    }
}