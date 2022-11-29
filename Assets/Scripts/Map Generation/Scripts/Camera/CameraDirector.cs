using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class CameraDirector : MonoBehaviour, ICameraDirector
{
    public  DirectorsCamera currentCamera { get; private set; }
    Dictionary<string, DirectorsCamera> cameras = new Dictionary<string, DirectorsCamera>();

    public void Initialize()
    {

    }

    public void RegisterCamera(DirectorsCamera directorsCamera)
    {
        cameras.Add(directorsCamera.name, directorsCamera);
    }

    public void SwitchMainCamera(string cameraName)
    {
        DirectorsCamera cam;
        if (cameras.TryGetValue(cameraName, out cam))
        {
            cam.Enable();
            currentCamera?.Disable();
            currentCamera = cam;
        }
    }

    [Inject]
    private void Construct(DirectorsCamera directorsCamera)
    {
        directorsCamera.RegisterCamera("MainCamera");
        SwitchMainCamera("MainCamera");
        directorsCamera.transform.SetParent(transform, false);
    }


}
