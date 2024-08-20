using System;
using System.Collections.Generic;
using UnityEngine;

public class CashList<T> : List<T>, IDisposable
{
    public CashList() : base() { }

    public CashList(int capacity) : base(capacity) { }

    public CashList(ICollection<T> coolection) : base(coolection) { }

    #region Fields

    static Stack<CashList<T>> _pool = new Stack<CashList<T>>();

    bool _inPool = false;

    #endregion Fields
    #region Methods

    public static CashList<T> Get()
    {
        var item = _pool.Count > 0 ? _pool.Pop() : new CashList<T>();
        item._inPool = false;
        return item;
    }

    public static CashList<T> Get(int capacity)
    {
        var item = _pool.Count > 0 ? _pool.Pop() : new CashList<T>(capacity);
        item._inPool = false;
        return item;
    }

    public static CashList<T> Get(ICollection<T> collection)
    {
        CashList<T> item;
        if (_pool.Count > 0)
        {
            item = _pool.Pop();
            item.AddRange(collection);
        }
        else
        {
            item = new CashList<T>(collection);
        }

        item._inPool = false;
        return item;
    }

    public static void Return(CashList<T> item)
    {
        if (!item._inPool)
        {
            item._inPool = true;
            item.Clear();
            _pool.Push(item);
        }
        else
        {
            Debug.LogError("Повторный возврат в пул кешируемого листа");
        }
    }

    public void Dispose() => Return(this);

    public override string ToString() => $"{Count} элементов {typeof(T)}";

    #endregion Methods
}
