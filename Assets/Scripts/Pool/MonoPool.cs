using System.Collections.Generic;
using UnityEngine;

public static class MonoPool
{
    static MonoPool()
    {
        var obj = new GameObject("MonoPoolRoot");
        _poolParent = obj.GetComponent<Transform>();
        Object.DontDestroyOnLoad(obj);
    }

    static Transform _poolParent;

    static Dictionary<PoolableMono, Stack<PoolableMono>> _pools = new Dictionary<PoolableMono, Stack<PoolableMono>>();

    public static T Get<T>(T origin) where T : PoolableMono
    {
        if (!_pools.TryGetValue(origin, out var pool))
        {
            pool = new Stack<PoolableMono>();
            _pools.Add(origin, pool);
        }

        var item = pool.Count > 0 ? (T)pool.Pop() : Object.Instantiate(origin);

        item.gameObject.SetActive(true);
        item.InPool = false;
        return item;
    }

    public static void Return<T>(T origin, T item) where T : PoolableMono
    {
        if (_pools.TryGetValue(origin, out var pool))
        {
            if (!item.InPool)
            {
                item.Clear();
                item.gameObject.SetActive(false);
                item.InPool = true;
                item.transform.SetParent(_poolParent, false);

                pool.Push(item);
            }
            else
            {
                Debug.LogError($"[MonoPool] Попытка повторного возврата в пул. Объект: {item.name}", item);
            }
        }
    }
}

public class PoolableMono : MonoBehaviour
{
    PoolableMono _origin;

    public bool InPool { get; set; }

    public PoolableMono Pop()
    {
        var origin = _origin ?? this;
        var item = MonoPool.Get(origin);
        item._origin = origin;
        item.gameObject.SetActive(true);

        return item;
    }

    public void Return() => MonoPool.Return(_origin, this);

    public virtual void Clear() { }
}

public static class MonoPoolExtention
{
    public static T Get<T>(this T origin) where T : PoolableMono => origin.Pop() as T;
}
