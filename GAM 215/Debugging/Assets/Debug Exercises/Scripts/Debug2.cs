using UnityEngine;
using System.Collections;

public class Debug2 : MonoBehaviour
{
	public GameObject guiObject = null;
	public float speed = 3.0f;
	
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
        velocity = Vector3.zero;
		
		if (Input.GetKey("w") == true)
		{
			velocity.z = speed;
		}
		else if (Input.GetKey("s") == true)
		{
			velocity.z = -speed;
		}
		
		if (Input.GetKey("a") == true)
		{
			transform.Rotate(0, -1, 0);
		}
		else if (Input.GetKey("d") == true)
		{
			transform.Rotate(0, 1, 0);
		}
		
		controller.Move(transform.forward * velocity.z * Time.deltaTime);
		
		guiObject.GetComponent<GUIText>().text = transform.position.ToString();
	}
}
