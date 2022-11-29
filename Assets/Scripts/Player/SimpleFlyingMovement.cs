using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using EvoCube.Player;

public class SimpleFlyingMovement : MonoBehaviour
{
    public Transform targetTransform;
    public float movementSpeed = 20f;
    private float speed;

    [Inject]
    void Contruct(Player player)
    {
        targetTransform = player.transform;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift))
            speed = movementSpeed * 4;
        else
            speed = movementSpeed;

        if (Input.GetKey(KeyCode.W))
            targetTransform.position += targetTransform.forward * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.S))
            targetTransform.position -= targetTransform.forward * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.A))
            targetTransform.position -= targetTransform.right * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.D))
            targetTransform.position += targetTransform.right * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.Space))
            targetTransform.position += Vector3.up * Time.deltaTime * speed;
        if (Input.GetKey(KeyCode.LeftControl))
            targetTransform.position -= Vector3.up * Time.deltaTime * speed;
    }
}


