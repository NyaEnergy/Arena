using System;
using UnityEngine;

[Serializable]
public struct RangeInt {
    [SerializeField] private int _min;
    [SerializeField] private int _max;

    public int Min => _min;
    public int Max => _max;

    public RangeInt(int min, int max) {
        _min = min;
        _max = max;
    }
}