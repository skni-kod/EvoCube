using UnityEngine;

public interface ICameraDirector
{
    Camera camera { get; }
    void Initialize();
}
