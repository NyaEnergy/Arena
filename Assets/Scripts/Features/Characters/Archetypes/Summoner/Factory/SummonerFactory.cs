using UnityEngine;

public class SummonerFactory {
    private readonly SummonerPool _pool;

    public SummonerFactory(SummonerPool pool) {
        _pool = pool;
    }

    public SummonerView Spawn(TeamType teamType,
                              SummonerConfig config,
                              Vector3 position) {
        if (config == null) return null;

        SummonerPoolKey key = new(teamType, config);
        SummonerView view = _pool.Get(key, position);

        if (view == null) return null;

        view.EnterBattlefield();
        return view;
    }
}