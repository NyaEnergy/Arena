using UnityEngine;
using UnityEngine.AI;

public class CharacterView : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private Collider _collider;

    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    public Collider Collider => _collider;

    public void Enable() {
        gameObject.SetActive(true);
        if (_agent != null) _agent.enabled = true;
        if (_collider != null) _collider.enabled = true;
    }

    public void Disable() {
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

    public void PlayAttack() {
        if (_animator == null) return;
        _animator.SetTrigger("Attack");
    }

    public void PlayDeath() {
        if(_animator == null) return;
        _animator.SetTrigger("Death");
    }
}
