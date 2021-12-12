using UnityEngine;

public class CameraSimpleController : MonoBehaviour
{
	Vector2 rotation = Vector2.zero;
	public float speed = 3f;
	public float movement_speed = 500f;

	void Update()
	{
		rotation.y += Input.GetAxis("Mouse X");
		rotation.x += -Input.GetAxis("Mouse Y");
		transform.eulerAngles = (Vector2)rotation * speed;
		if (Input.GetKey(KeyCode.W))
        {
			transform.position += transform.forward * 0.6f * movement_speed * Time.deltaTime;
        }
		if (Input.GetKey(KeyCode.S))
		{
			transform.position -= transform.forward * 0.6f * movement_speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.Space))
		{
			transform.position += transform.up * 0.4f * movement_speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.LeftControl))
		{
			transform.position -= transform.up * 0.4f * movement_speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.A))
		{
			transform.position -= transform.right * 0.8f * movement_speed * Time.deltaTime;
		}
		if (Input.GetKey(KeyCode.D))
		{
			transform.position += transform.right * 0.8f * movement_speed * Time.deltaTime;
		}
	}
}
