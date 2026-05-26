using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Territory/Territory Config",fileName = "TerritoryConfig")]
public class TerritoryConfig : ScriptableObject {
    [SerializeField] private string _territoryName = "Toxic Swamp";

    [SerializeField] private float _mediumPressureTime = 90f;
    [SerializeField] private float _highPressureTime = 180f;
    [SerializeField] private float _criticalPressureTime = 300f;

    [SerializeField] private float _pressureGrowthMultiplier = 1.15f;

    public string TerritoryName => _territoryName;

    public float MediumPressureTime => _mediumPressureTime;
    public float HighPressureTime => _highPressureTime;
    public float CriticalPressureTime => _criticalPressureTime;

    public float PressureGrowthMultiplier => _pressureGrowthMultiplier;
}
