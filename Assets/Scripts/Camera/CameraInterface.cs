using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraInterface : MonoBehaviour
{
    private static CameraInterface instance = null;
    public static new Camera camera;
    public static Vector3 centerPointOfCameraOnTheFloor = Vector3.zero;

    static Plane plane = new Plane(new Vector3(0, 1, 0).normalized, Vector3.zero);
    Rect screenRect = new Rect(0, 0, Screen.width, Screen.height);

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            GameObject.DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            GameObject.Destroy(this.gameObject);
        }
        camera = GetComponent<Camera>();
    }


    private void GetCenterPointOfCameraONTheFloor()
    {

        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width/2 , Screen.height/2, 0));

        float enter = 0.0f;

        if (plane.Raycast(ray, out enter))
        {
            centerPointOfCameraOnTheFloor = ray.GetPoint(enter);
        }
    }

    private void Update()
    {
        GetCenterPointOfCameraONTheFloor();
    }

}
