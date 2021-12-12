using System.Collections;
using System.Collections.Concurrent;



public static class ConcurrentDictionaryExtension
{

    public static void ForcedAdd<Tkey, T>(this ConcurrentDictionary<Tkey, T> dict, Tkey key, T item)
    {
        bool succes = false;
        int loop_counter = 0;
        while (!succes)
        {
            succes = dict.TryAdd(key, item);
            loop_counter++;
            if (loop_counter > 50)
                throw new System.Exception($"ForcedAdd failed {loop_counter}times. Terminating loop. <Tkey>key, <T>item = <{typeof(Tkey)}>{key},<{typeof(T)}>{item}. Queue: {dict.Keys}:{dict.Values}");
        }
    }


}



