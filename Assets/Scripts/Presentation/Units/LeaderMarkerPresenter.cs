using ConveyorWars.Presentation.Input;
using UnityEngine;

namespace ConveyorWars.Presentation.Units {
    public sealed class LeaderMarkerPresenter {
        private const float GROUND_PROBE_HEIGHT = 1.5f;
        private const float GROUND_PROBE_DISTANCE = 3f;
        private const float SURFACE_OFFSET = 0.025f;

        private readonly GameplayControlConfig _config;

        private PlaceholderAllyView _leaderView;

        public LeaderMarkerPresenter(GameplayControlConfig config) {
            _config = config;
        }

        public void SetLeader(PlaceholderAllyView view) {
            if (_leaderView == view) return;

            if (_leaderView != null &&
                _leaderView.LeaderRing != null) {
                _leaderView.LeaderRing.enabled = false;
            }

            _leaderView = view;

            if (_leaderView == null ||
                _leaderView.LeaderRing == null) {
                return;
            }

            _leaderView.LeaderRing.enabled = true;
            UpdateTransform();
        }

        public void Tick() {
            if (_leaderView == null ||
                _leaderView.LeaderRing == null ||
                !_leaderView.LeaderRing.enabled) {
                return;
            }

            UpdateTransform();
        }

        private void UpdateTransform() {
            Vector3 origin = _leaderView.transform.position +
                             Vector3.up * GROUND_PROBE_HEIGHT;

            if (!Physics.Raycast(
                    origin,
                    Vector3.down,
                    out RaycastHit hit,
                    GROUND_PROBE_DISTANCE,
                    _config.GroundMask,
                    QueryTriggerInteraction.Ignore)) {
                return;
            }

            Transform ringTransform = _leaderView.LeaderRing.transform;

            ringTransform.position = hit.point +
                                     hit.normal * SURFACE_OFFSET;

            ringTransform.rotation =
                Quaternion.FromToRotation(
                    Vector3.up,
                    hit.normal);
        }
    }
}