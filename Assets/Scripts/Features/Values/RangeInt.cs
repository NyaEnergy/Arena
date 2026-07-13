using System;
using UnityEngine;

[Serializable]
public struct RangeInt {
    [SerializeField] private Vector2 _value;

    public float Min => Mathf.Min(_value.x, _value.y);
    public float Max => Mathf.Max(_value.x, _value.y);

    public RangeInt(float min, float max) {
        _value = new Vector2(min, max);
    }
}