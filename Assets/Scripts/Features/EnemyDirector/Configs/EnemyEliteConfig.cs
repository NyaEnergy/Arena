using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Enemy Commander/Enemy Elite Config",
                 fileName = "EnemyEliteConfig")]
public sealed class EnemyEliteConfig : ScriptableObject {
    [SerializeField] private CharacterType _characterType = CharacterType.EliteCommander;
    [SerializeField] private Sprite _icon;

    public CharacterType CharacterType => _characterType;
    public Sprite Icon => _icon;

    public bool IsValid => _characterType == CharacterType.EliteCommander &&
                           _icon != null;
}
