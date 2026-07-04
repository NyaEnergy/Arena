using System.Collections;
using UnityEngine;

public class HunterView : CharacterView {
    [SerializeField] private Transform _attackOrigin;
    [SerializeField] private LineRenderer _shotTracer;
    [SerializeField] private float _shotDuration = 0.08f;

    private Coroutine _shotRoutine;

    public override CharacterType CharacterType => CharacterType.Hunter;

    private void OnDisable() {
        StopShot();
    }

    public void PlayRangedAttack(CharacterView target) {
        PlayAttack(target);

        if (target == null ||
            _shotTracer == null) return;

        StopShot();

        _shotRoutine = StartCoroutine(
                       ShowShot(target));
    }

    public void PlayMeleeAttack(CharacterView target) {
        PlayAttack(target);
    }

    private IEnumerator ShowShot(CharacterView target) {
        _shotTracer.positionCount = 2;
        _shotTracer.useWorldSpace = true;
        _shotTracer.enabled = true;

        float endTime = Time.time + _shotDuration;

        while (Time.time < endTime && target != null) {
            _shotTracer.SetPosition(
                0, GetAttackOriginPosition());

            _shotTracer.SetPosition(
                1, target.AimPosition);

            yield return null;
        }

        _shotTracer.enabled = false;
        _shotRoutine = null;
    }

    private Vector3 GetAttackOriginPosition() {
        return _attackOrigin != null ?
            _attackOrigin.position : AimPosition;
    }

    private void StopShot() {
        if (_shotRoutine != null) {
            StopCoroutine(_shotRoutine);
            _shotRoutine = null;
        }

        if (_shotTracer != null)
            _shotTracer.enabled = false;
    }
}
