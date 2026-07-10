using UnityEngine;

public class MinionView : CharacterView {
    [Header("Combat")]
    [SerializeField] private ParticleSystem _attackEffect;

    [Header("Spawn")]
    [SerializeField] private ParticleSystem _spawnEffect;

    public override CharacterType CharacterType => CharacterType.Minion;

    public void PlaySpawn() => RestartEffect(_spawnEffect);

    public override void PlayAttack(CharacterView target) {
        base.PlayAttack(target);
        RestartEffect(_attackEffect);
    }

    private void RestartEffect(ParticleSystem effect) {
        if (effect == null) return;

        effect.Stop(
            true, ParticleSystemStopBehavior.StopEmittingAndClear);

        effect.Play(true);
    }
}