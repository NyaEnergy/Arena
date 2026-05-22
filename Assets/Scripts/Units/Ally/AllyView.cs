using UnityEngine;

public class AllyView : CharacterView {
    [SerializeField] private Renderer _bodyRenderer;
    public Renderer BodyRenderer => _bodyRenderer;
}
