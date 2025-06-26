using UnityEngine;
using System.Collections.Generic;

public class SDictionary<T1, T2> : Dictionary<T1, T2>
{
    new public T2 this[T1 key]
    {
        set
        {
            if (value == null)
            {
                Remove(key);
            }
            else
            {
                base[key] = value;
            }
        }
        get
        {
            if (TryGetValue(key, out T2 value))
            {
                return value;
            }
            return default;
        }
    }

    public SDictionary() : base() { }

    public SDictionary(IDictionary<T1, T2> dictionary) : base(dictionary) { }

    private List<T1> needRemove = new List<T1>();

    public void RemoveFromCondition(System.Func<T2, bool> condition = null)
    {
        needRemove.Clear();
        foreach (var kvp in this)
        {
            if (condition?.Invoke(kvp.Value) == true)
            {
                needRemove.Add(kvp.Key);
            }
        }

        foreach (var key in needRemove)
        {
            Remove(key);
        }
    }

    public void RemoveFromCondition(System.Func<T1, bool> condition = null)
    {
        needRemove.Clear();
        foreach (var kvp in this)
        {
            if (condition?.Invoke(kvp.Key) == true)
            {
                needRemove.Add(kvp.Key);
            }
        }

        foreach (var key in needRemove)
        {
            Remove(key);
        }
    }

    public void LogContents()
    {
        string logMessage = "SDictionary contents:\n";
        foreach (var kvp in this)
        {
            logMessage += $"Key: {kvp.Key}, Value: {kvp.Value}\n";
        }
        Debug.Log(logMessage);
    }
}
