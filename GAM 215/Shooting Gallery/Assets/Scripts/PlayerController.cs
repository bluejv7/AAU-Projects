using UnityEngine;
using System.Collections;

/// <summary>
/// Handles calls relating to the player object and handling events (like movement)
/// </summary>
public class PlayerController : MonoBehaviour {
    #region Enums or structs

    /// <summary>
    /// The types of weapons we'll have
    /// </summary>
    private enum WEAPON
    {
        RAYCAST_PISTOL, SPREAD_SHOT
    };

    #endregion

    #region Reference Variables

    // This header is useless (won't show up in debug), but if, for some strange reason,
    // I decide to attach a serialized field reference variable, this will be useful
    [Header("--- Reference Variables ---")]

    /// <summary>
    /// Reference variable for the player's rigidbody
    /// </summary>
    private Rigidbody playerRigidbody = null;

    /// <summary>
    /// The spread shot we will spawn when firing that weapon
    /// </summary>
    [SerializeField] private GameObject spreadShot = null;

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

    /// <summary>
    /// The amount of damage the player does
    /// </summary>
    [SerializeField] private int damage = 1;

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

    /// <summary>
    /// Reference variable for the ray we will cast
    /// </summary>
    private Ray ray;

    /// <summary>
    /// Reference variable for the raycast return
    /// </summary>
    private RaycastHit hitInfo;

    /// <summary>
    /// Reference variable for the hitInfo collider
    /// </summary>
    private Collider hitCollider = null;

    /// <summary>
    /// The vector representing the center of the screen (probably should be some constant, but not sure how to do it in C#)
    /// </summary>
    private Vector3 centerVector = new Vector3(0.5f, 0.5f, 0);

    /// <summary>
    /// The current weapon we are using
    /// </summary>
    private WEAPON currentWeapon = WEAPON.RAYCAST_PISTOL;

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

        // Go through inputs to see if we need to switch weapons
        HandleWeaponSwitching();

        // FireWeapon when mouse1 is down
        if (Input.GetMouseButtonDown(0))
        {
            FireWeapon();
        }
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Handle all the cases where we switch weapons
    /// </summary>
    private void HandleWeaponSwitching()
    {
        // If 1, switch to raycast pistol
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentWeapon = WEAPON.RAYCAST_PISTOL;
        }
        // If 2, switch to spread shot
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            currentWeapon = WEAPON.SPREAD_SHOT;
        }
    }

    /// <summary>
    /// Fire the weapon
    /// </summary>
    private void FireWeapon()
    {
        // Call different firing functions depending on the weapon fired
        if (currentWeapon == WEAPON.RAYCAST_PISTOL)
        {
            FireRaycastPistol();
        }
        // TODO: Introduce delay for this weapon
        else if (currentWeapon == WEAPON.SPREAD_SHOT)
        {
            FireSpreadShot();
        }
    }

    /// <summary>
    /// Fire raycast pistol
    /// </summary>
    private void FireRaycastPistol()
    {
        // Initialize the ray
        ray = Camera.main.ViewportPointToRay(centerVector);

        // cast the ray
        // Note: didn't create a reference variable for the bool return, since I figured a bool takes up so little memory (but not sure if that's true)
        bool didHit = Physics.Raycast(ray, out hitInfo);

        // Send event to object if hit
        if (didHit)
        {
            hitCollider = hitInfo.collider;
            if (hitCollider.tag == "Enemy")
            {
                hitCollider.SendMessage("OnShot", damage);
            }
        }
    }

    /// <summary>
    /// Fire spread shot -- this will fire 5 bullets in an arc
    /// </summary>
    private void FireSpreadShot()
    {
        Instantiate(spreadShot, Camera.main.ScreenToWorldPoint(Input.mousePosition), Camera.main.transform.rotation);
    }

    /// <summary>
    /// Kill the player
    /// </summary>
    public void Die()
    {
        // Detach all children
        this.transform.DetachChildren();

        // Destroy player object
        Destroy(this.gameObject);
    }

    #endregion
}
