using System.Collections.Generic;

public class SummonerConfigCollection {
    private readonly HashSet<SummonerConfig>
        _configs = new();

    public SummonerConfigCollection(IReadOnlyList<SummonerConfig> configs) {
        if (configs == null) return;

        for (int i = 0; i < configs.Count; i++) {
            SummonerConfig config = configs[i];

            if (config != null)
                _configs.Add(config);
        }
    }

    public bool Contains(SummonerConfig config) {
        return config != null &&
              _configs.Contains(config);
    }
}