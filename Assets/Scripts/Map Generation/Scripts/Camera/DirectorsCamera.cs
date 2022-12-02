using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DirectorsCamera : MonoBehaviour
{
    [Inject] readonly ICameraDirector _cameraDirector;
    public new Camera camera;
    public AudioListener audioListener;
    public string cameraName;
    public int id;

    [Inject] void Construct()
    {
        camera = gameObject.AddComponent<Camera>();
        audioListener = gameObject.AddComponent<AudioListener>();
        Disable();
    }

    public void RegisterCamera(string cameraName)
    {
        setName(cameraName);
        _cameraDirector.RegisterCamera(this);
    }
    public void SetActive()
    {
        Guard.AgainstNull(cameraName, "Camera doesn't have a name");
        _cameraDirector.SwitchMainCamera(cameraName);
    }

    private void setName(string name)
    {
        gameObject.name = name;
        cameraName = name;
    }

    public void Enable()
    {
        camera.enabled = true;
        audioListener.enabled = true;
    }

    public void Disable()
    {
        camera.enabled = false;
        audioListener.enabled = false;
    }

}
