using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Commanders/Ally Commander Config",
                 fileName = "AllyCommanderConfig")]
public sealed class AllyCommanderConfig : CommanderConfig {
    public override TeamType TeamType => TeamType.Ally;
}
