using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColosionHandler : MonoBehaviour
{
    private void OnColisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Finish")
        {
            Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaa");
        }
    }
}
