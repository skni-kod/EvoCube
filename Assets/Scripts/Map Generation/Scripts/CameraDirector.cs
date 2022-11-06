using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;


public class CameraDirector : MonoBehaviour, ICameraDirector
{
    public new Camera camera { get; private set; }

    public void Initialize()
    {
        createMainCamera();
    }

    private void createMainCamera()
    {
        GameObject cameraObj = new GameObject("Camera");
        cameraObj.transform.parent = transform;
        cameraObj.AddComponent<Camera>();
        cameraObj.AddComponent<AudioListener>();
    }

    [Inject]
    private void Init()
    {

    }


}
