using UnityEngine;
using UnityEngine.AI;

public class CharacterView : MonoBehaviour {
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _agent;

    public Animator Animator => _animator;
    public NavMeshAgent Agent => _agent;
}
