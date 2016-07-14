using UnityEngine;
using System.Collections;

/// <summary>
/// Handles calls relating to the player object and handling events (like movement)
/// </summary>
public class PlayerController : MonoBehaviour {
    #region Reference Variables

    // This header is useless (won't show up in debug), but if, for some strange reason,
    // I decide to attach a serialized field reference variable, this will be useful
    [Header("--- Reference Variables ---")]

    /// <summary>
    /// Reference variable for the player's rigidbody
    /// </summary>
    private Rigidbody playerRigidbody = null;

    #endregion

    #region Input Axis Names

    [Header("--- Input Axis Names ---")]

    /// <summary>
    /// Input axis name for moving forward or backwards
    /// </summary>
    [SerializeField] private string forwardBackwardInputName = "Vertical";

    /// <summary>
    /// Input axis name for strafing
    /// </summary>
    [SerializeField] private string strafingInputName = "Horizontal";

    #endregion

    #region Player Configurations

    [Header("--- Player Configs ---")]

    /// <summary>
    /// The forward and backward speeds of the player
    /// </summary>
    [SerializeField] private float forwardBackwardSpeed = 5.0f;

    /// <summary>
    /// The strafing speed of the player
    /// </summary>
    [SerializeField] private float strafingSpeed = 4.0f;

    #endregion

    #region Calculation Variables
    
    [Header("--- Calculation Variables ---")]

    /// <summary>
    /// Handle calculations for the velocity -- also useful so we don't create/destroy variables every update
    /// (at least, that's what I'm assuming)
    /// </summary>
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// The velocity in relation to the direction the character is facing
    /// </summary>
    private Vector3 localVelocity = Vector3.zero;

    #endregion

    #region Event Handlers

    /// <summary>
    /// Handle player movement
    /// </summary>
    /// <param name="forwardBackwardInput">The magnitude/direction of the character in the Z axis</param>
    /// <param name="strafingInput">The magnitude/direction of the character in the X axis</param>
    private void MovePlayer(float forwardBackwardInput, float strafingInput)
    {
        // Handle forward backward movement
        velocity.z = forwardBackwardInput * forwardBackwardSpeed;

        // Handle strafing left/right
        velocity.x = strafingInput * strafingSpeed;
    }

    /// <summary>
    /// Initialize reference variables and some configurations
    /// </summary>
    private void Start()
    {
        playerRigidbody = this.GetComponent<Rigidbody>();
        
        // freeze rotation to prevent character from falling over
        playerRigidbody.freezeRotation = true;
    }

    /// <summary>
    /// Handles player updates every frame
    /// </summary>
    private void Update()
    {
        // Reset velocity for X, Z (even though the character doesn't jump, I might add that in later when I get more time, so I won't reset Y)
        velocity.x = 0;
        velocity.z = 0;

        // Handle player movement
        float forwardBackwardInput = Input.GetAxis(forwardBackwardInputName);
        float strafingInput = Input.GetAxis(strafingInputName);
        MovePlayer(forwardBackwardInput, strafingInput);

        // Adjust the velocity relative to where the character is facing
        localVelocity = this.transform.TransformDirection(velocity);

        // Apply the velocity
        playerRigidbody.velocity = localVelocity;
    }

    #endregion
}
