using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EvoCube.Player;
using Zenject;

public class FirstPersonCamera : MonoBehaviour
{
    public Transform targetTransform;
    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;
    public float sensitivityX = 2f;
    public float sensitivityY = 2f;

    public float minimumX = -360f;
    public float maximumX = 360f;

    public float minimumY = -60f;
    public float maximumY = 60f;

    float rotationY = 0f;

    [Inject] void Construct(Player player)
    {
        targetTransform = player.transform;
    }

    void Update()
    {
        if (axes == RotationAxes.MouseXAndY)
        {
            float rotationX = targetTransform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            targetTransform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }
        else if (axes == RotationAxes.MouseX)
        {
            targetTransform.Rotate(0, Input.GetAxis("Mouse X") * sensitivityX, 0);
        }
        else
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            targetTransform.localEulerAngles = new Vector3(-rotationY, targetTransform.localEulerAngles.y, 0);
        }
    }
}
