using UnityEngine;

public class CharacterVisualReset {
    private static readonly int IdleStateHash =
        Animator.StringToHash("Base Layer.Idle");

    private readonly Transform _root;
    private readonly Animator _animator;

    private readonly Renderer[] _renderers;
    private readonly bool[] _rendererStates;

    private readonly ParticleSystem[] _particles;

    private readonly Transform _animatedTransform;
    private readonly Vector3 _animatedPosition;
    private readonly Quaternion _animatedRotation;
    private readonly Vector3 _animatedScale;

    public CharacterVisualReset(Transform root,
                                Animator animator) {
        _root = root;
        _animator = animator;

        _renderers = root.GetComponentsInChildren<Renderer>(true);

        _rendererStates = new bool[_renderers.Length];

        for (int i = 0; i < _renderers.Length; i++) {
            _rendererStates[i] = _renderers[i].enabled;
        }

        _particles = root.GetComponentsInChildren<ParticleSystem>(true);

        if (_animator == null) return;

        _animatedTransform = _animator.transform;
        _animatedPosition = _animatedTransform.localPosition;
        _animatedRotation = _animatedTransform.localRotation;
        _animatedScale = _animatedTransform.localScale;
    }

    public void Hide() {
        for (int i = 0; i < _renderers.Length; i++) {
            if (_renderers[i] != null) {
                _renderers[i].enabled = false;
            }
        }
    }

    public void Show() {
        for (int i = 0; i < _renderers.Length; i++) {
            if (_renderers[i] != null) {
                _renderers[i].enabled =
                    _rendererStates[i];
            }
        }
    }

    public void Reset() {
        StopParticles();
        ResetAnimator();
    }

    private void ResetAnimator() {
        if (_animator == null) return;

        Vector3 worldPosition = _root.position;
        Quaternion worldRotation = _root.rotation;
        Vector3 rootScale = _root.localScale;

        _animator.enabled = true;
        _animator.Rebind();

        ResetTriggers();

        if (_animator.HasState(0, IdleStateHash)) {
            _animator.Play(IdleStateHash, 0, 0f);
        }

        _animator.Update(0f);

        if (_animatedTransform != _root) {
            _animatedTransform.localPosition = _animatedPosition;
            _animatedTransform.localRotation = _animatedRotation;
            _animatedTransform.localScale = _animatedScale;
        }

        _root.SetPositionAndRotation(worldPosition,
                                     worldRotation);

        _root.localScale = rootScale;
    }

    private void ResetTriggers() {
        AnimatorControllerParameter[] parameters =
            _animator.parameters;

        for (int i = 0; i < parameters.Length; i++) {
            if (parameters[i].type != AnimatorControllerParameterType.Trigger)
                continue;

            _animator.ResetTrigger(parameters[i].nameHash);
        }
    }

    private void StopParticles() {
        for (int i = 0; i < _particles.Length; i++) {
            if (_particles[i] == null) continue;

            _particles[i].Stop(true,
                ParticleSystemStopBehavior
                    .StopEmittingAndClear);
        }
    }
}