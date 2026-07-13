using UnityEngine;

[CreateAssetMenu(menuName = "Configs/UI/Health Bar Palette",
                 fileName = "HealthBarPalette")]
public class HealthBarPaletteConfig : ScriptableObject {
    [Header("Ally")]
    [SerializeField] private Color _allyBackground = new(0.02f, 0.10f, 0.12f, 0.9f);
    [SerializeField] private Color _allyFill = new(0.1f, 0.85f, 0.95f, 1f);

    [Header("Enemy")]
    [SerializeField] private Color _enemyBackground = new(0.14f, 0.04f, 0.02f, 0.9f);
    [SerializeField] private Color _enemyFill = new(1f, 0.22f, 0.08f, 1f);

    public Color GetBackground(TeamType teamType) {
        return teamType == TeamType.Ally ?
            _allyBackground : _enemyBackground;
    }

    public Color GetFill(TeamType teamType) {
        return teamType == TeamType.Ally ?
            _allyFill : _enemyFill;
    }
}