using UnityEngine;

public class VanguardView : CharacterView {
    [SerializeField] private ParticleSystem _attackEffect;
    public override void PlayAttack(CharacterView target) {
        base.PlayAttack(target);
        if (_attackEffect == null) return;
        _attackEffect.Play();
    }
}
