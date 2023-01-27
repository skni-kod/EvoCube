using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Zenject;

public class UiDirector : MonoBehaviour, IUiDirector
{
    public Canvas MainCanvas { get; private set; }
    [Inject] CoreDirector coreDirector;
    Dictionary<string, GameObject> uiPrefabs = new Dictionary<string, GameObject>();

    [Inject] void Construct()
    {
        createEventSystem();
        createMainCanvas();
    }

    private void createMainCanvas()
    {
        GameObject canvasObject = new GameObject("MainCanvas");
        Canvas canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        MainCanvas = canvas;
        canvasObject.transform.parent = transform;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<VerticalLayoutGroup>();
        canvasObject.AddComponent<GraphicRaycaster>();

        loadUIPrefab("MainMenu");

    }

    public void loadUIPrefab(string prefabName)
    {
        GameObject part = Instantiate((GameObject)Resources.Load($"UIPrefabs/{prefabName}"));
        uiPrefabs[prefabName] = part;
        UIPrefab prefab = part.GetComponent<UIPrefab>();
        prefab.SetDirector(this);
        prefab.SetCore(coreDirector);
        part.transform.parent = MainCanvas.transform;
    }

    public void UnloadPrefab(string name)
    {
        uiPrefabs[name].SetActive(false);
    }

    public void ActivatePrefab(string name)
    {
        uiPrefabs[name].SetActive(true);
    }

    void createEventSystem()
    {
        GameObject eventSystem = new GameObject("EventSystem");
        eventSystem.transform.parent = transform;
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
    }

    [Obsolete("Idk, dont use that")]
    public void Initialize()
    {

    }
}
