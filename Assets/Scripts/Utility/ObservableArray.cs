using System;
using System.Collections;
using System.Collections.Generic;

public class ObservableArray<T> : IEnumerable<T>
{
    private T[] array;
    public event Action<int, T> OnValueChanged;

    public ObservableArray(int size)
    {
        array = new T[size];
    }

    public T this[int index]
    {
        get => array[index];
        set
        {
            array[index] = value;
            OnValueChanged?.Invoke(index, value);
        }
    }

    public int Length => array.Length;

    public IEnumerator<T> GetEnumerator()
    {
        foreach (var item in array)
            yield return item;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}
