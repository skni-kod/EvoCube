using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Movment :  IMovment
{
    public  int speed{get;set;}
    public int speedObrotu { get; set; }
	public Vector2 rotation { get; set; }
	public Transform transform { get; set; }
	public Vector3 moveDirection { get; set; }
	public float horizontalInput { get; set; }
	public float verticalInput { get; set; }
	[Inject]
	public void construct(Playermovment playermovment)
    {
		transform = playermovment.transform;
		speed = 14;
		speedObrotu = 50;
		rotation = Vector2.zero;

		
    }

 
	public void MyInput()
    {
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");
    }
}
