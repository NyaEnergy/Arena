using ConveyorWars.Presentation.Units;
using DG.Tweening;
using UnityEngine;

namespace ConveyorWars.Presentation.Combat {
    public sealed class UnitAttackPresenter {
        private const float ATTACK_OFFSET = 0.16f;
        private const float ATTACK_FORWARD_DURATION = 0.07f;
        private const float ATTACK_RETURN_DURATION = 0.11f;

        private const float HIT_SCALE = 0.08f;
        private const float HIT_DURATION = 0.16f;

        private readonly UnitView _view;
        private readonly Transform _body;

        private readonly Vector3 _defaultLocalPosition;
        private readonly Vector3 _defaultLocalScale;

        private Tween _attackTween;
        private Tween _hitTween;

        public UnitAttackPresenter(UnitView view) {
            _view = view;
            _body = view.Body;

            _defaultLocalPosition = _body.localPosition;
            _defaultLocalScale = _body.localScale;
        }

        public void PlayAttack(Vector3 targetPosition) {
            Vector3 direction = targetPosition - _view.transform.position;

            direction.y = 0f;

            if (direction.sqrMagnitude <= Mathf.Epsilon) return;

            _attackTween?.Kill();

            _body.localPosition = _defaultLocalPosition;

            Vector3 localDirection =
                _body.parent.InverseTransformDirection(
                    direction.normalized);

            Vector3 attackPosition = _defaultLocalPosition +
                                     localDirection * ATTACK_OFFSET;

            _attackTween = DOTween.Sequence()
                           .Append(_body.DOLocalMove(
                                   attackPosition,
                                   ATTACK_FORWARD_DURATION))

                           .Append(_body.DOLocalMove(
                                   _defaultLocalPosition,
                                   ATTACK_RETURN_DURATION))
                           
                           .SetLink(_view.gameObject,
                                    LinkBehaviour.KillOnDisable);
        }

        public void PlayHit() {
            _hitTween?.Kill();

            _body.localScale = _defaultLocalScale;

            _hitTween = _body.DOPunchScale(
                                Vector3.one * HIT_SCALE,
                                HIT_DURATION,
                                1, 0f)
                             .SetLink(_view.gameObject,
                                       LinkBehaviour.KillOnDisable);
        }
    }
}
