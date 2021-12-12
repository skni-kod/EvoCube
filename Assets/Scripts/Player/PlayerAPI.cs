using System.Collections;
using UnityEngine;


public class PlayerAPI : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private Transform playerPosition;
    public static PlayerAPI instance;

    protected virtual void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public static Vector3 GetPlayerPosition()
    {
        return instance.playerPosition.position;
    }

}
