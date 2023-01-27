using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using UnityEngine.SceneManagement;

public class CoreDirector : MonoBehaviour
{
    [Inject] UiDirector uiDirector;

    public void LoadAnotherScene(int number)
    {
        SceneManager.LoadScene(number, LoadSceneMode.Additive);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void Action(int number)
    {
        switch(number)
        { 
            case 0: SceneManager.LoadScene(7, LoadSceneMode.Single); break;
            case 1: QuitGame(); break;
            case 2: uiDirector.UnloadPrefab("MainMenu"); break;
        }

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            uiDirector.ActivatePrefab("MainMenu"); 
            
        }
    }
}
