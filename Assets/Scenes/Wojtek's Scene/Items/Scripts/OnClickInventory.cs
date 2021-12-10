using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OnClickInventory : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public GameObject panel;
    GraphicRaycaster raycaster;
    [SerializeField] public UnityEvent e;
    private bool isOn;
    private bool mouseIsOver;
    public delegate void OnBlikEndEvent();

    void Start()
    {
        isOn = panel.active;
    }
    void Awake()
    {
        // Get both of the components we need to do this
        this.raycaster = GetComponent<GraphicRaycaster>();
    }
    void Update()
    {

        if (isOn)
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                Deactive();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Deactive();
            }
            if (!mouseIsOver)
            {
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Deactive();
                }
            }

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.I))
            {
                panel.SetActive(true);
                isOn = true;
            }
        }
    }

   

    public void OnPointerEnter(PointerEventData eventData)
    {
        mouseIsOver = true;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseIsOver = false;
        EventSystem.current.SetSelectedGameObject(gameObject);
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(gameObject);
    }


    private void Deactive()
    {
        panel.SetActive(false);
        isOn = false;
    }
}
