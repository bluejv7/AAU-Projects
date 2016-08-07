using UnityEngine;
using System.Collections;

/// <summary>
/// Handle events and updates for the camera
/// </summary>
public class CameraController : MonoBehaviour {
    /// <summary>
    /// Reference variable for player
    /// </summary>
    private GameObject player = null;

    /// <summary>
    /// Our initial position
    /// </summary>
    private Vector3 initialPosition = Vector3.zero;

    /// <summary>
    /// New position to update our transform with
    /// </summary>
    private Vector3 newPosition = Vector3.zero;

	/// <summary>
    /// Initialize variables
    /// </summary>
	private void Start()
    {
        player = GameObject.FindWithTag("Player");
        initialPosition = this.transform.position;
	}
	
	/// <summary>
    /// Handle what happens each frame
    /// </summary>
	private void Update()
    {
        // Exit early if player dies
        if (!player) return;

        newPosition.x = player.transform.position.x;
        newPosition.y = initialPosition.y;
        newPosition.z = player.transform.position.z + initialPosition.z;
        this.transform.position = newPosition;
	}
}
