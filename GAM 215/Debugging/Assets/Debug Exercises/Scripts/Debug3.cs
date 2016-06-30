using UnityEngine;
using System.Collections;

public class Debug3 : MonoBehaviour
{
	public GameObject guiObject = null;
	public float speed = 3.0f;
	public float jumpSpeed = 5.0f;
	
	private CharacterController controller = null;
	private Vector3 velocity = Vector3.zero;

	// Use this for initialization
	void Start ()
	{
		controller = this.GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update ()
	{
        velocity.x = 0;
        velocity.z = 0;

		if (Input.GetKey("w"))
		{
			velocity.z = speed;
		}
		else if (Input.GetKey("s"))
		{
			velocity.z = -speed;
		}
		
		if (Input.GetKey("a"))
		{
			transform.Rotate (0, -1, 0);
		}
		else if (Input.GetKey("d"))
		{
			transform.Rotate (0, 1, 0);
		}
		
		if (Input.GetKeyDown ("space"))
		{
			velocity.y = jumpSpeed;
		}
        velocity += Physics.gravity * Time.deltaTime;
		
		controller.Move (	transform.forward * velocity.z * Time.deltaTime +
							transform.up * velocity.y * Time.deltaTime);
		
		guiObject.GetComponent<GUIText>().text = transform.position.ToString();
	}
}
