using UnityEngine;

public interface ICameraDirector
{
    DirectorsCamera currentCamera { get; }
    void Initialize();

    void RegisterCamera(DirectorsCamera directorsCamera);
    void SwitchMainCamera(string cameraName);
}
