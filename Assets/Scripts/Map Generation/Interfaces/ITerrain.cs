using UnityEngine;


namespace EvoCube.MapGeneration
{
    public interface ITerrain
    {
        void Initialize();
        void SetTargetForGeneration(Transform target);
    }
}
