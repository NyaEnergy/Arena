using UnityEngine;
using UnityEngine.AI;

public abstract class CharacterView : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Collider _collider;
    [SerializeField] private HealthBarView _healthBarView;
    [SerializeField] private Transform _aimPoint;
    [SerializeField] private ParticleSystem _hitEffect;

    private CharacterLifecycleController _lifecycleController;
    private CharacterVisualReset _visualReset;

    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    public Collider Collider => _collider;
    public HealthBarView HealthBarView => _healthBarView;

    public CharacterBrain Brain => _lifecycleController?.Brain;

    public Vector3 AimPosition => _aimPoint != null ?
                                  _aimPoint.position :
                                  transform.position + Vector3.up;

    private void Awake() {
        _visualReset = new CharacterVisualReset(transform, _animator);
    }

    private void Update() {
        _lifecycleController?.Tick();
    }

    private void OnDestroy() {
        _lifecycleController?.Dispose();
        _lifecycleController = null;
    }

    public bool Initialize(CharacterLifecycleController controller) {
        if (_lifecycleController != null ||
            controller == null) {
            return false;
        }

        _lifecycleController = controller;
        return true;
    }

    public void OnSpawned() {
        _lifecycleController?.OnSpawned();
    }

    public void OnDespawned() {
        _lifecycleController?.OnDespawned();
    }

    public void EnterBattlefield(CharacterPresenceTransitionRequest? request = null) {
        if (request.HasValue) {
            _lifecycleController?.EnterBattlefield(
                request.Value);

            return;
        }

        _lifecycleController?.EnterBattlefield();
    }

    public void ExitBattlefield(CharacterPresenceTransitionRequest? request = null) {
        if (request.HasValue) {
            _lifecycleController?.ExitBattlefield(
                request.Value);

            return;
        }

        _lifecycleController?.ExitBattlefield();
    }

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        SetNavigationEnabled(false);
        gameObject.SetActive(false);
    }

    public void SetNavigationEnabled(bool isEnabled) {
        if (_agent == null ||
            _agent.enabled == isEnabled) {
            return;
        }

        if (!isEnabled &&
            _agent.enabled &&
            _agent.isOnNavMesh) {
                _agent.ResetPath();
        }

        _agent.enabled = isEnabled;
    }

    public void ResetVisualState() {
        _visualReset?.Reset();
    }

    public void HidePresentation() {
        _visualReset?.Hide();
    }

    public void ShowPresentation() {
        _visualReset?.Show();
    }

    public virtual void PlayAttack( CharacterView target) {
        if (_animator != null) {
            _animator.SetTrigger("Attack");
        }
    }

    public void PlayHit() {
        if (_hitEffect == null) return;

        _hitEffect.Stop(true, ParticleSystemStopBehavior
                              .StopEmittingAndClear);

        _hitEffect.Play(true);
    }

    public void PlayDeath() {
        if (_animator != null) {
            _animator.SetTrigger("Death");
        }
    }
}