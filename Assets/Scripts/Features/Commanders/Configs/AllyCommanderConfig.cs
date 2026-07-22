using UnityEngine;

[CreateAssetMenu(menuName = "Configs/Commanders/Allies/Commander Config",
                 fileName = "AllyCommanderConfig")]
public sealed class AllyCommanderConfig : CommanderConfig {
    public override TeamType TeamType => TeamType.Ally;
}
