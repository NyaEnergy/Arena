using UnityEngine;

public class CharacterCameraVisibilityService {
    private readonly Camera _camera;

    public CharacterCameraVisibilityService(Camera camera) {
        _camera = camera;
    }

    public bool IsVisible(CharacterView view,
                          Vector3 position) {
        if (_camera == null) return false;

        Bounds bounds =
            GetBounds(view, position);

        if (IsVisible(bounds.center)) {
            return true;
        }

        Vector3 min = bounds.min;
        Vector3 max = bounds.max;

        for (int x = 0; x <= 1; x++) {
            for (int y = 0; y <= 1; y++) {
                for (int z = 0; z <= 1; z++) {
                    Vector3 point =
                        new(x == 0 ? min.x : max.x,
                            y == 0 ? min.y : max.y,
                            z == 0 ? min.z : max.z);

                    if (IsVisible(point)) {
                        return true;
                    }
                }
            }
        }

        return false;
    }

    private Bounds GetBounds(CharacterView view,
                             Vector3 position) {
        if (view?.Collider == null) {
            return new Bounds(position + Vector3.up * 0.5f,
                              Vector3.one);
        }

        Bounds bounds = view.Collider.bounds;

        bounds.center += position -
                         view.transform.position;

        return bounds;
    }

    private bool IsVisible(Vector3 position) {
        Vector3 viewport = _camera.WorldToViewportPoint(position);

        return viewport.z > 0f &&
               viewport.x >= 0f &&
               viewport.x <= 1f &&
               viewport.y >= 0f &&
               viewport.y <= 1f;
    }
}