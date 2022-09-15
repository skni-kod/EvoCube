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
        //GameObject cameraObj = Instantiate(cameraPrefab);
        //cameraObj.transform.SetParent(transform);
        //camera = cameraObj.GetComponent<Camera>();
    }

    [Inject]
    private void Init()
    {

    }


}
