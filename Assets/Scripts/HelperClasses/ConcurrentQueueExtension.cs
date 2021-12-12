using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;

public static class ConcurrentQueueExtension
{

    public static List<T> ForcedClear<T>(this ConcurrentQueue<T> q)
    {
        List<T> items = new List<T>();
        int loop_counter = 0;
        while (!q.IsEmpty)
        {
            T item;
            if (q.TryDequeue(out item))
                items.Add(item);
            else
                loop_counter++;
            if (loop_counter > 50)
                throw new System.Exception($"ForcedClear failed {loop_counter}times. Terminating loop. Item: {item}. Queue: {q}");
        }
        return items;
    }


}
