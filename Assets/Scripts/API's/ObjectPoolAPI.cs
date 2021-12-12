using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;


public interface IObjectPool
{
    public void Init();
}

public static class ObjectPoolAPI
{
    private static ConcurrentDictionary<int, object> pool_registry = new ConcurrentDictionary<int, object>();

    public static void Register<T>(ObjectPool<T> obj) where T : IObjectPool, new()
    {
        obj.Id = pool_registry.Count;
        bool succes = false;
        while (!succes)
            succes = pool_registry.TryAdd(pool_registry.Count, obj);
    }
}

public class ObjectPool <T> where T : IObjectPool, new()
{ 
    public int Id;
    public int size;
    public int maxSize;
    private bool _registered = false;

    public ConcurrentQueue<T> container;

    public ObjectPool(int _size, int _maxSize = 8)
    {
        container = new ConcurrentQueue<T>();
        size = _size;
        maxSize = Mathf.Max(_maxSize, size);
        Init();
    }

    private void Init()
    {
        if (_registered)
            throw new System.Exception($"Already Initialized Object Pool: id: {Id}, type container: {typeof(T)}");
        for (int i = 0;i < size;i++)
        {
            T obj = new T();
            obj.Init();
            container.Enqueue(obj);
        }
        ObjectPoolAPI.Register<T>(this);
    }

}
