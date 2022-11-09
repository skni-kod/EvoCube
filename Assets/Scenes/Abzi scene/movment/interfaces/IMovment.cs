using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovment 
{
    int speed { get; set; }
    int speedObrotu { get; set; }
    Vector2 rotation { get; set; }
    Vector3 moveDirection { get; set; }
    float horizontalInput { get; set; }

    float verticalInput { get; set; }
    public void Ruch();
    public void MyInput();
}
