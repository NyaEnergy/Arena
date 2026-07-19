using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ControllerFieldPool {
    private readonly DiContainer _container;
    private readonly ControllerConfig _config;
    private readonly Queue<ControllerFieldView> _pool = new();

    public ControllerFieldPool(DiContainer container,
                               ControllerConfig config) {

        _container = container;
        _config = config;
    }

    public ControllerFieldView Get(Vector3 position,
                                   float radius,
                                   Color color) {

        ControllerFieldView prefab = _config.FieldPrefab;

        if (prefab == null) return null;

        ControllerFieldView view = _pool.Count > 0 ?
            _pool.Dequeue() : _container
                              .InstantiatePrefabForComponent
                              <ControllerFieldView>(prefab);

        if (view == null) return null;

        view.Show(position, radius, color);
        return view;
    }

    public void Return(ControllerFieldView view) {

        if (view == null) return;

        view.Hide();
        _pool.Enqueue(view);
    }
}