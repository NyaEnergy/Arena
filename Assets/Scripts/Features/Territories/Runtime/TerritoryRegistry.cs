using System.Collections.Generic;

public class TerritoryRegistry {
    private readonly List<TerritoryRuntime> _territories = new();

    public IReadOnlyList<TerritoryRuntime> Territories => _territories;

    public void Register(TerritoryRuntime territory) {
        if (territory == null ||
            territory.View == null ||
            _territories.Contains(territory)) {
            return;
        }

        _territories.Add(territory);
    }

    public bool TryGet(TerritoryView view,
                       out TerritoryRuntime territory) {
        territory = null;

        if (view == null) return false;

        for (int i = 0; i < _territories.Count; i++) {
            TerritoryRuntime current = _territories[i];

            if (current?.View != view) continue;

            territory = current;
            return true;
        }

        return false;
    }

    public void Clear() {
        _territories.Clear();
    }
}