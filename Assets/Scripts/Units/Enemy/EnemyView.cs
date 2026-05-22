using UnityEngine;

public class EnemyView : CharacterView {
    [SerializeField] private Renderer _bodyRenderer;
    public Renderer BodyRenderer => _bodyRenderer;
}
