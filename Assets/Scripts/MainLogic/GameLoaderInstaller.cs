using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class GameLoaderInstaller : MonoInstaller
{
    public override void InstallBindings()
    {
        SceneManager.LoadScene("Core", LoadSceneMode.Single);
    }

}
