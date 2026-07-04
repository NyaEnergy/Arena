using UnityEngine;

public class MedicView : CharacterView {
    [SerializeField] private Transform _healingOrigin;
    [SerializeField] private LineRenderer _healingLine;

    private CharacterView _healingTarget;

    public override CharacterType CharacterType => CharacterType.Medic;

    private void LateUpdate() {
        UpdateHealingLine();
    }

    private void OnDisable() {
        ClearHealingTarget();
    }

    public void SetHealingTarget(CharacterView target) {
        _healingTarget = target;

        if (_healingLine == null) return;

        _healingLine.positionCount = 2;
        _healingLine.useWorldSpace = true;
        _healingLine.enabled = _healingTarget != null;
    }

    public void ClearHealingTarget() {
        _healingTarget = null;

        if (_healingLine != null)
            _healingLine.enabled = false;
    }

    private void UpdateHealingLine() {
        if (_healingLine == null ||
            _healingTarget == null) return;

        _healingLine.SetPosition(
            0, GetHealingOriginPosition());

        _healingLine.SetPosition(
            1, _healingTarget.AimPosition);
    }

    private Vector3 GetHealingOriginPosition() {
        return _healingOrigin != null ?
               _healingOrigin.position : AimPosition;
    }
}
