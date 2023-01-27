using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIPrefab : MonoBehaviour
{
    [SerializeField] public string nameID = "";
    UiDirector uiDirector;
    CoreDirector coreDirector;
    [SerializeField] public string[] actions = new string[0];


    public void SetDirector(UiDirector director)
    {
        uiDirector = director;
    }

    public void SetCore(CoreDirector core)
    {
        coreDirector = core;
    }

    public void ButtonAction(int number)
    {
        coreDirector.Action(number);
    }

}
