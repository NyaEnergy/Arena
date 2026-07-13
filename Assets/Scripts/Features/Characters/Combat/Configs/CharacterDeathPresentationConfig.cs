using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Characters/Death Presentation",
                 fileName = "CharacterDeathPresentation")]
public class CharacterDeathPresentationConfig : ScriptableObject {
    [SerializeField] private float _animationDuration = 1f;
    [SerializeField] private float _bodyDuration = 2.5f;

    public float AnimationDuration => _animationDuration;

    public float BodyDuration => _bodyDuration;

    public float TotalDuration => Mathf.Max(0f, _animationDuration) +
                                  Mathf.Max( 0f, _bodyDuration);
}