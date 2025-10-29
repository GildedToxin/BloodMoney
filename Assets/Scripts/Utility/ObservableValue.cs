using System;
using UnityEngine;

[Serializable]
public class ObservableValue<T>
{
    [SerializeField] private T _value;
    public event Action<T> OnValueChanged;

    public T Value
    {
        get => _value;
        set
        {
            if (!Equals(_value, value))
            {
                _value = value;
                OnValueChanged?.Invoke(_value);
            }
        }
    }

    public ObservableValue(T initialValue = default)
    {
        _value = initialValue;
    }

    public static implicit operator T(ObservableValue<T> observable) => observable.Value;
}