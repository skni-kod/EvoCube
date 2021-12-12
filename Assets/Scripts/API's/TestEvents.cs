using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public delegate void OnCharacterDied();

public class TestEvents : MonoBehaviour
{


    
    public event OnCharacterDied onChampDie;
    

    private void Awake()
    {
        onChampDie += CloseMenu;
        onChampDie -= CloseMenu;
        onChampDie += DisplayDeathMessage;
        onChampDie += DisplayDeathMessage;
    }

    private void Start()
    {
        onChampDie?.Invoke();
    }

    private void DisplayDeathMessage()
    {
        Debug.Log("Umarłeś lol");
    }

    private void CloseMenu()
    {
        Debug.Log("Test");
    }


}
