using UnityEngine;
using System.Collections;

/// <summary>
/// Defines the character controller for the player
/// </summary>
public class Debug1 : MonoBehaviour
{
    /// <summary>
    /// The reference to the `CharacterController` component
    /// </summary>
    private CharacterController controller = null;

    /// <summary>
    /// The velocity to update the player with
    /// </summary>
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// The speed we want the character to move at
    /// </summary>
    [SerializeField] private float speed = 5.0f;

    /// <summary>
    /// Initialize some references
    /// </summary>
    private void Start()
    {
        controller = this.GetComponent<CharacterController>();
    }

    /// <summary>
    /// Updates to player movement when "w" is pressed
    /// </summary>
    void Update()
    {
        // Zero out velocities that we don't want to be applied
        velocity.x = 0;
        velocity.z = 0;

        // Move forward
        if (Input.GetKey("w"))
        {
            velocity.z = speed;
        }

        // Apply the movement to the player
        controller.Move(transform.forward * velocity.z * Time.deltaTime);
    }
}
