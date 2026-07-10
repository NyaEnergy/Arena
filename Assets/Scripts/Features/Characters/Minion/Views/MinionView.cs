using UnityEngine;

public class MinionView : CharacterView {
    [Header("Combat")]
    [SerializeField] private ParticleSystem _attackEffect;

    [Header("Spawn")]
    [SerializeField] private ParticleSystem _spawnEffect;

    public override CharacterType CharacterType => CharacterType.Minion;

    public void PlaySpawn() {
        if (_spawnEffect == null) return;

        _spawnEffect.Play();
    }

    public override void PlayAttack(CharacterView target) {
        base.PlayAttack(target);

        if (_attackEffect == null) return;

        _attackEffect.Play();
    }
}