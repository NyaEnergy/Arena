using System;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.UI;
using Zenject;

public class DemoBattleCallService : IInitializable,
                                     IDisposable {
    private readonly IReadOnlyList <DemoBattleCallEntry> _entries;

    private readonly DemoBattleCallSpawnService _spawnService;

    private readonly Dictionary <Button, UnityAction> _actions = new();

    public DemoBattleCallService(
            IReadOnlyList<DemoBattleCallEntry> entries,
            DemoBattleCallSpawnService spawnService) {
        
        _entries = entries;
        _spawnService = spawnService;
    }

    public void Initialize() {
        for (int i = 0; i < _entries.Count; i++) {
            Bind(_entries[i]);
        }
    }

    public void Dispose() {
        foreach (var pair in _actions) {
            if (pair.Key != null) {
                pair.Key
                    .onClick
                    .RemoveListener(pair.Value);
            }
        }

        _actions.Clear();
    }

    private void Bind(DemoBattleCallEntry entry) {
        if (entry == null || !entry.IsValid ||
            _actions.ContainsKey(entry.Button))
                return;

        UnityAction action = () => _spawnService.Spawn(entry);
        entry.Button.onClick.AddListener(action);
        _actions.Add(entry.Button, action);
    }
}