using System.Collections.Generic;
using UnityEngine;

public static class Pool<T> where T : class, IPoolable, new()
{
    static Stack<T> _stack = new Stack<T>();

    public static T Get()
    {
        var item = _stack.Count > 0 ? _stack.Pop() : new T();
        item.InPool = false;
        return item;
    }

    public static void Return(T item)
    {
        if (!item.InPool)
        {
            item.InPool = true;
            item.Clean();
            _stack.Push(item);
        }
        else Debug.LogError($"[Pool] Попытка повторного возврата в пул. Тип объекта: {item.GetType()}");
    }
}
