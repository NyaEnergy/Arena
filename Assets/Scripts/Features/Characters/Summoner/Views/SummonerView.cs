using UnityEngine;

public class SummonerView : CharacterView {
    [Header("Summoning")]
    [SerializeField] private Transform _summonOrigin;
    [SerializeField] private ParticleSystem _summonEffect;

    public override CharacterType CharacterType => CharacterType.Summoner;

    public Vector3 SummonOriginPosition =>
        _summonOrigin != null
            ? _summonOrigin.position
            : AimPosition;

    public void PlaySummon() {
        if (Animator != null) {
            Animator.SetTrigger("Summon");
        }

        if (_summonEffect == null) return;

        if (_summonOrigin != null) {
            _summonEffect.transform.position = _summonOrigin.position;
            _summonEffect.transform.rotation = _summonOrigin.rotation;
        }

        _summonEffect.Play();
    }
}