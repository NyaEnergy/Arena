using UnityEngine;

public class MinionView : CharacterView {
    [SerializeField] private ParticleSystem _attackEffect;

    public override void PlayAttack(CharacterView target) {
        base.PlayAttack(target);

        if (_attackEffect == null) return;

        _attackEffect.Stop(true, ParticleSystemStopBehavior
                                 .StopEmittingAndClear);

        _attackEffect.Play(true);
    }
}