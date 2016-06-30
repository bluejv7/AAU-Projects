using UnityEngine;
using System.Collections;

/// <summary>
/// Controller for handling player movement and updating text with our transform position
/// </summary>
public class Debug3 : MonoBehaviour
{
    /// <summary>
    /// A reference to the text `GameObject`
    /// </summary>
	[SerializeField] private GameObject guiObject = null;

    /// <summary>
    /// The forward/backward speed of the player
    /// </summary>
	[SerializeField] private float speed = 3.0f;

    /// <summary>
    /// The jump speed of the player
    /// </summary>
	[SerializeField] private float jumpSpeed = 5.0f;
	
    /// <summary>
    /// A reference to the `CharacterController` component
    /// </summary>
	private CharacterController controller = null;

    /// <summary>
    /// The velocity we want to update each `Update()` cycle
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
    /// Handle how/when player moves, apply gravity and jumping, and update `GUIText` with our transform position
    /// </summary>
	private void Update ()
	{
        // Zero out velocities we want to update (we don't zero out Y, because we want to preserve gravity's affects)
        velocity.x = 0;
        velocity.z = 0;

        // Move forward
		if (Input.GetKey("w"))
		{
			velocity.z = speed;
		}
        // Move backward
		else if (Input.GetKey("s"))
		{
			velocity.z = -speed;
		}
		
        // Look left
		if (Input.GetKey("a"))
		{
			transform.Rotate (0, -1, 0);
		}
        // Look right
		else if (Input.GetKey("d"))
		{
			transform.Rotate (0, 1, 0);
		}
		
        // Jump
		if (Input.GetKeyDown ("space"))
		{
			velocity.y = jumpSpeed;
		}

        // Apply gravity
        velocity += Physics.gravity * Time.deltaTime;
		
        // Apply the velocities to our character
		controller.Move (	transform.forward * velocity.z * Time.deltaTime +
							transform.up * velocity.y * Time.deltaTime);
		
        // Update the `GUIText` with our transform position
		guiObject.GetComponent<GUIText>().text = transform.position.ToString();
	}
}
