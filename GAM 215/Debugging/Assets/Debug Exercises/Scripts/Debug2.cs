using UnityEngine;
using System.Collections;

/// <summary>
/// Defines the character controller for the player
/// </summary>
public class Debug2 : MonoBehaviour
{
    /// <summary>
    /// The gui `GameObject` to reference
    /// </summary>
	[SerializeField] private GameObject guiObject = null;

    /// <summary>
    /// The speed we want to move forward and backwards with
    /// </summary>
    [SerializeField] private float speed = 3.0f;
	
    /// <summary>
    /// The reference to our `CharacterController` component
    /// </summary>
	private CharacterController controller = null;

    /// <summary>
    /// The velocity to update the player with
    /// </summary>
    private Vector3 velocity = Vector3.zero;

	/// <summary>
    /// Initialize reference variables
    /// </summary>
	private void Start ()
	{
        controller = this.GetComponent<CharacterController>();
	}
	
	/// <summary>
    /// Decides how and when to move our character based on inputs and also update the `GUIText`
    /// </summary>
	private void Update ()
	{
        // Zero out velocities we want to update ourselves
        velocity.x = 0;
        velocity.z = 0;
		
        // Move forward
		if (Input.GetKey("w") == true)
		{
			velocity.z = speed;
		}
        // Move backward
		else if (Input.GetKey("s") == true)
		{
			velocity.z = -speed;
		}
		
        // Look left
		if (Input.GetKey("a") == true)
		{
			transform.Rotate(0, -1, 0);
		}
        // Look right
		else if (Input.GetKey("d") == true)
		{
			transform.Rotate(0, 1, 0);
		}
		
        // Apply the movement to the player
		controller.Move(transform.forward * velocity.z * Time.deltaTime);
		
        // Update the `GUItext` to show our transform position
		guiObject.GetComponent<GUIText>().text = transform.position.ToString();
	}
}
