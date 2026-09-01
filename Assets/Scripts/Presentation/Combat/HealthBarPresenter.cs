using ConveyorWars.Units.Combat;
using UnityEngine;

namespace ConveyorWars.Presentation.Combat {
    public sealed class HealthBarPresenter {
        private readonly IHealthReadOnly _health;
        private readonly HealthBarView _view;
        private readonly Camera _camera;

        public HealthBarPresenter(IHealthReadOnly health,
                                  HealthBarView view,
                                  Camera camera) {
            _health = health;
            _view = view;
            _camera = camera;
        }

        public void LateTick() {
            if (_health.MaxHealth <= 0) return;

            _view.Fill.fillAmount =
                Mathf.Clamp01((float)_health.CurrentHealth /
                                     _health.MaxHealth);

            _view.transform.rotation = _camera.transform.rotation;
        }
    }
}
