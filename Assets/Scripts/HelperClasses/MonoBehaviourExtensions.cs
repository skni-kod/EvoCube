
using UnityEngine;

public static class MonoBehaviourExtensions
{
    public static void Adopt<T>(this T q, GameObject gameObject) where T : Component
    {
        gameObject.transform.parent = q.transform;
    }
    public static void Adopt<T>(this T q, Transform transform) where T : Component
    {
        transform.parent = q.transform;
    }
}