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
	//	rb = playermovment.GetComponent<Rigidbody>();
		
    }
    public void Ruch()
    {
		/*
		if (Input.GetKey(KeyCode.W))
		{
			transform.position += transform.forward * 0.6f * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.S))
		{
			transform.position -= transform.forward * 0.6f * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			transform.position += transform.up * 0.4f * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			transform.position -= transform.up * 0.4f * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.position -= transform.right * 0.8f * speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.position += transform.right * 0.8f * speed * Time.deltaTime;
		} */
	

	}
 
	public void MyInput()
    {
		horizontalInput = Input.GetAxisRaw("Horizontal");
		verticalInput = Input.GetAxisRaw("Vertical");
    }
}
