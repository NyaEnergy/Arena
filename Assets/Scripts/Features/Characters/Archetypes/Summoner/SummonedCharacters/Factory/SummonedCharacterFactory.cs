using UnityEngine;

public class SummonedCharacterFactory {
    private readonly SummonedCharacterPool _pool;

    public SummonedCharacterFactory(SummonedCharacterPool pool) {
        _pool = pool;
    }

    public CharacterView Prepare(TeamType teamType,
                                 SummonedCharacterConfig config,
                                 Vector3 position) {

        if (config == null) return null;

        SummonedCharacterPoolKey key = new(teamType, config);

        return _pool.Get(key, position);
    }

    public CharacterView Spawn(TeamType teamType,
                               SummonedCharacterConfig config,
                               Vector3 position) {
        CharacterView view =
            Prepare(teamType,
                    config,
                    position);

        if (view == null) return null;

        view.EnterBattlefield();
        return view;
    }

    public void Return(CharacterView view) {
        _pool.Return(view);
    }
}