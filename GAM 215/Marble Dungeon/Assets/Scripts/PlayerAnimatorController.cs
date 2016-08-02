using UnityEngine;
using System.Collections;

/// <summary>
/// Handle the animations of the player
/// </summary>
public class PlayerAnimatorController : MonoBehaviour {
    /// <summary>
    /// Player's rigidbody
    /// </summary>
    [SerializeField] private Rigidbody playerRigidbody = null;

    /// <summary>
    /// Player's animator
    /// </summary>
    [SerializeField] private Animator playerAnimator = null;

    /// <summary>
    /// Velocity in reference to the player's direction
    /// </summary>
    private Vector3 localVelocity = Vector3.zero;

	/// <summary>
    /// Initialize variables
    /// </summary>
	private void Start()
    {
        playerRigidbody = this.GetComponent<Rigidbody>();
        playerAnimator = this.GetComponent<Animator>();
	}
	
	/// <summary>
    /// Handle animation events every frame
    /// </summary>
	private void Update()
    {
        // Get the local velocity and figure out if we moved
        localVelocity = this.transform.TransformDirection(playerRigidbody.velocity);
        playerAnimator.SetFloat("RightLeft", localVelocity.x);
        playerAnimator.SetFloat("UpDown", localVelocity.z);
	}
}
