using UnityEngine;

public class TurretView : CharacterView {
    [SerializeField] private Transform _rotationRoot;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private ParticleSystem _attackEffect;
    [SerializeField] private LineRenderer _shotLine;

    public void RotateTo(CharacterView target) {
        if (target == null) return;

        Transform root = _rotationRoot != null ?
            _rotationRoot : transform;

        Vector3 direction = target.AimPosition -
                            root.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return;

        root.rotation = Quaternion.LookRotation(
                        direction.normalized,
                        Vector3.up);
    }

    public override void PlayAttack(CharacterView target) {
        base.PlayAttack(target);

        if (_attackEffect == null) {
            return;
        }

        _attackEffect.Stop(true,
            ParticleSystemStopBehavior
                .StopEmittingAndClear);

        _attackEffect.Play(true);
    }

    public void ShowShot(Vector3 targetPosition) {
        if (_shotLine == null) return;

        Vector3 startPosition = _muzzle != null ?
            _muzzle.position : AimPosition;

        _shotLine.positionCount = 2;
        _shotLine.SetPosition(0, startPosition);
        _shotLine.SetPosition(1, targetPosition);
        _shotLine.enabled = true;
    }

    public void HideShot() {
        if (_shotLine != null) {
            _shotLine.enabled = false;
        }
    }
}