using System;
using UnityEngine;

[Serializable]
public struct Range {
    [SerializeField] private float _min;
    [SerializeField] private float _max;

    public float Min => _min;
    public float Max => _max;

    public Range(float min, float max) {
        _min = min;
        _max = max;
    }
}