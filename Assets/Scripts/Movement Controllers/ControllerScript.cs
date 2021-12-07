using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllerScript : MonoBehaviour
{
    public float speed = 2;
    private MultiLegWalkerCode walkerScript;
    public Vector3 direction = Vector3.zero;

    void Start()                                                                    
    {                                                                               
        walkerScript = GetComponent<MultiLegWalkerCode>();                          
    }                                                                               
    void Update()                                                                   
    {                                                                               

        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A)) { direction = CameraInterface.camera.transform.parent.forward - CameraInterface.camera.transform.parent.right; }
        else if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D)) { direction = CameraInterface.camera.transform.parent.forward + CameraInterface.camera.transform.parent.right; }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D)) { direction = -CameraInterface.camera.transform.parent.forward + CameraInterface.camera.transform.parent.right; }
        else if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A)) { direction = -CameraInterface.camera.transform.parent.forward - CameraInterface.camera.transform.parent.right; }

        else if (Input.GetKey(KeyCode.W)) { direction = CameraInterface.camera.transform.parent.forward; }
        else if (Input.GetKey(KeyCode.S)) { direction = -CameraInterface.camera.transform.parent.forward; }
        else if (Input.GetKey(KeyCode.A)) { direction = -CameraInterface.camera.transform.parent.right; }
        else if (Input.GetKey(KeyCode.D)) { direction = CameraInterface.camera.transform.parent.right; }
        else direction = Vector3.zero;

        walkerScript.MoveDirection = direction;
	}
}