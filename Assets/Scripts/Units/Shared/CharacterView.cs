using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterView : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Collider _collider;
    [SerializeField] private HealthBarView _healthBarView;
    [SerializeField] private Transform _aimPoint;
    [SerializeField] private ParticleSystem _hitEffect;

    private CharacterController _controller;

    public abstract CharacterType CharacterType { get; }

    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    public Collider Collider => _collider;
    public HealthBarView HealthBarView => _healthBarView;
    public CharacterBrain Brain => _controller?.Brain;
    public CharacterKey CharacterKey => _controller != null ?
                                        _controller.CharacterKey : default;

    public Vector3 AimPosition {
        get => _aimPoint != null ?
               _aimPoint.position : transform.position +
                                    Vector3.up;
    }

    private void Update() {
        _controller?.Tick();
    }

    private void OnDestroy() {
        _controller?.Dispose();
        _controller = null;
    }

    public bool Initialize(CharacterController controller) {

        if (_controller != null ||
            controller == null) {

            return false;
        }

        _controller = controller;
        return true;
    }

    public void OnSpawned() {
        _controller?.OnSpawned();
    }

    public void OnDespawned() {
        _controller?.OnDespawned();
    }

    public void EnterBattlefield() {
        _controller?.EnterBattlefield();
    }

    public void Show() {
        gameObject.SetActive(true);

        if (_agent != null) _agent.enabled = true;
        if (_collider != null) _collider.enabled = true;
    }

    public void Hide() {
        if (_agent != null && _agent.enabled) {
            if (_agent.isOnNavMesh) _agent.ResetPath();
            _agent.enabled = false;
        }

        if (_collider != null) _collider.enabled = false;

        gameObject.SetActive(false);
    }

    public void ResetAnimationState() {
        if (_animator == null) return;
        _animator.Rebind();
        _animator.Update(0f);
    }

    public virtual void PlayAttack(CharacterView target) {
        if (_animator == null) return;
        _animator.SetTrigger("Attack");
    }

    public void PlayHit() {
        if (_hitEffect == null) return;
        _hitEffect.Play();
    }

    public void PlayDeath() {
        if (_animator == null) return;
        _animator.SetTrigger("Death");
    }
}
