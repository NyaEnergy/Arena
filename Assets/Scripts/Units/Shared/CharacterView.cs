using UnityEngine;
using UnityEngine.AI;

public class CharacterView : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private CapsuleCollider _capsuleCollider;

    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
    public CapsuleCollider CapsuleCollider => _capsuleCollider;
}
