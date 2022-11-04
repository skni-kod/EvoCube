using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiDirector : MonoBehaviour, IUiDirector
{

    public void CreateCanvas()
    {

    }

    void createEventSystem()
    {
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.transform.parent = transform;
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
    }

    public void Initialize()
    {

    }
}
