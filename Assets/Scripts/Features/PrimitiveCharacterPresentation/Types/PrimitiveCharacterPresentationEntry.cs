using UnityEngine;

[System.Serializable]
public class PrimitiveCharacterPresentationEntry {
    [SerializeField] private CharacterType _characterType;
    [SerializeField] private PrimitiveType _primitiveType;

    [SerializeField] private Vector3 _localPosition;
    [SerializeField] private Vector3 _localScale = Vector3.one;

    public CharacterType CharacterType => _characterType;
    public PrimitiveType PrimitiveType => _primitiveType;

    public Vector3 LocalPosition => _localPosition;
    public Vector3 LocalScale => _localScale;
}
