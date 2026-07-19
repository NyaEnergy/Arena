public class TerritoryRuntime {
    public TerritoryView View { get; }
    public bool IsSpawnEnabled { get; private set; }

    public TerritoryRuntime(TerritoryView view) {
        View = view;
    }

    public void EnableSpawn() {
        IsSpawnEnabled = true;
    }

    public void DisableSpawn() {
        IsSpawnEnabled = false;
    }
}