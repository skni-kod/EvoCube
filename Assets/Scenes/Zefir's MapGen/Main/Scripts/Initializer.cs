using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

public class Initializer : MonoBehaviour
{
    [Inject][SerializeField] IUiDirector uiDirector;
    [Inject][SerializeField] ICameraDirector cameraDirector;

    private void Start()
    {
        //SceneManager.LoadScene("UI", LoadSceneMode.Additive);
        uiDirector.Initialize();
        cameraDirector.Initialize();
    }
}
