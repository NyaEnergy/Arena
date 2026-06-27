public class PoolReuseDebugSettings {
    public int RespawnCount { get; }
    public float KillDelay { get; }
    public float RespawnDelay { get; }
    public float DespawnTimeout { get; }

    public PoolReuseDebugSettings(int respawnCount,
                                  float killDelay,
                                  float respawnDelay,
                                  float despawnTimeout) {
        RespawnCount = respawnCount;
        KillDelay = killDelay;
        RespawnDelay = respawnDelay;
        DespawnTimeout = despawnTimeout;
    }
}
