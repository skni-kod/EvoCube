using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;
using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;


public interface IPooledObject
{
    /// <summary>
    /// Initializes object
    /// </summary>
    public void Init();
    /// <summary>
    /// All buffers should be included here, or rather all the stuff that needs to be destroyed -> remember, memory leaks!
    /// </summary>
    public void OnRelease();
    public void Reload();
}

public static class ObjectPoolAPI
{
    private static ConcurrentDictionary<int, object> pool_registry = new ConcurrentDictionary<int, object>();

    public static void Register<T>(ObjectPool<T> obj) where T : IPooledObject, new()
    {
        obj.Id = pool_registry.Count;
        pool_registry.ForcedAdd(pool_registry.Count, obj);
    }
}

public class ObjectPool <T> where T : IPooledObject, new()
{ 
    public int Id;
    public int size;
    public int maxSize;
    private bool _registered = false;

    private ConcurrentQueue<T> container;

    public ObjectPool(int _size, int _maxSize = 8)
    {
        container = new ConcurrentQueue<T>();
        size = _size;
        maxSize = Mathf.Max(_maxSize, size);
        Init();
    }

    public void ReloadPool()
    {
        List<T> items = container.ForcedClear();
        items.ForEach(item => item.Reload());
        items.ForEach(item => container.Enqueue(item));
    }

    public void ClearPool()
    {
        List<T> items = container.ForcedClear();
        items.ForEach(item => item.OnRelease());
    }

    public bool GetOne(out T item)
    {
        return container.TryDequeue(out item);
    }

    public void ReturnIntoPool(T item)
    {
        container.Enqueue(item);
    }

    public void RepopulatePool()
    {
        for (int i = 0; i < size && container.Count < size; i++)
        {
            T obj = new T();
            obj.Init();
            container.Enqueue(obj);
        }
    }

    private void Init()
    {
        if (_registered)
            throw new System.Exception($"Already Initialized Object Pool: id: {Id}, type container: {typeof(T)}");
        RepopulatePool();
        ObjectPoolAPI.Register<T>(this);
    }

}
