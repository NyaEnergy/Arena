using System.Collections.Generic;
using Zenject;

public class TerritoryStartService : IInitializable {
    private readonly IReadOnlyList<TerritoryView> _views;
    private readonly TerritoryRegistry _registry;

    public TerritoryStartService(IReadOnlyList<TerritoryView> views,
                                 TerritoryRegistry registry) {
        _views = views;
        _registry = registry;
    }

    public void Initialize() {
        _registry.Clear();

        for (int i = 0; i < _views.Count; i++) {
            TerritoryView view = _views[i];

            if (view == null) continue;

            TerritoryRuntime runtime = new TerritoryRuntime(view);

            runtime.EnableSpawn();
            _registry.Register(runtime);
        }
    }
}