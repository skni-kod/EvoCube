using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Events;

public class MenuEvent : MonoBehaviour
{
    public delegate void OnMenuClose();
    public event OnMenuClose MenuClose;

    UnityEvent onMenuClose;
/*    void Start()
    {
        if (onMenuClose == null)
            onMenuClose = new UnityEvent();

        onMenuClose.AddListener(MenuClose);
    }

    void Update()
    {
        if (Input.anyKeyDown && onMenuClose != null)
        {
            onMenuClose.Invoke();
        }
    }

    void MenuClose()
    {
        Debug.Log("MenuClose");
    }*/
}
