using System.Collections;
using UnityEngine;


public class PlayerAPI : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
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
        return PlayerAPI.instance.playerObject.transform.position;
    }

    public static GameObject GetPlayerObject()
    {
        return PlayerAPI.instance.playerObject;
    }

}
