using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controller for main character (using `Rigidbody`)
/// </summary>
public class CharacterRigidBodyController : MonoBehaviour
{
    // Setup headers
    [Header("Input Axis Names")]

    // serialized private variables

    /// <summary>
    /// The health of our player
    /// </summary>
    [SerializeField] private int health = 100;

    /// <summary>
    /// The text `GameObject` on the canvas
    /// </summary>
    [SerializeField] private Text healthText;

    /// <summary>
    /// The text `GameObject` to indicate win/loss condition
    /// </summary>
    [SerializeField] private Text endingText;

    /// <summary>
    /// Object used for vertical tilting of the camera
    /// </summary>
    [SerializeField] private Transform cameraTarget = null;

    /// <summary>
    /// Minimum tilt value for the x-rotation of the camera
    /// </summary>
    [SerializeField] private float minTilt = -50.0f;

    /// <summary>
    /// Maximum tilt value fo the x-rotation of the camera
    /// </summary>
    [SerializeField] private float maxTilt = 30.0f;

    /// <summary>
    /// How sensitive the camera is when looking left/right
    /// </summary>
    [SerializeField] private float cameraSensitivityX = 2.5f;

    /// <summary>
    /// How sensitive the camera is when looking up/down
    /// </summary>
    [SerializeField] private float cameraSensitivityY = 2.0f;

    // Movement properties

    /// <summary>
    /// The velocity we will use when moving forward
    /// </summary>
    [SerializeField] private float forwardVelocity = 4.0f;

    /// <summary>
    /// The velocity we will use when moving forward in the air
    /// </summary>
    [SerializeField] private float forwardAerialVelocity = 2.5f;

    /// <summary>
    /// The velocity we will use when moving backwards
    /// </summary>
    [SerializeField] private float backwardVelocity = 4.0f;

    /// <summary>
    /// The velocity we will use when moving backwards in the air
    /// </summary>
    [SerializeField] private float backwardAerialVelocity = 2.5f;

    /// <summary>
    /// The velocity we will use when strafing left
    /// </summary>
    [SerializeField] private float leftStrafeVelocity = 4.0f;

    /// <summary>
    /// The velocity we will use when strafing left in the air
    /// </summary>
    [SerializeField] private float leftStrafeAerialVelocity = 2.5f;

    /// <summary>
    /// The velocity we will use when strafing right
    /// </summary>
    [SerializeField] private float rightStrafeVelocity = 4.0f;

    /// <summary>
    /// The velocity we will use when strafing right in the air
    /// </summary>
    [SerializeField] private float rightStrafeAerialVelocity = 2.5f;

    /// <summary>
    /// The jump velocity
    /// </summary>
    [SerializeField] private float jumpVelocity = 4.0f;

    /// <summary>
    /// Input axis name for forward/backward input
    /// </summary>
    [SerializeField] private string moveForwardBackwardInput = "Vertical";

    /// <summary>
    /// Input axis name for left/right strafe input
    /// </summary>
    [SerializeField] private string moveLeftRightInput = "Horizontal";

    /// <summary>
    /// Input axis name for jump input
    /// </summary>
    [SerializeField] private string jumpInput = "Jump";

    // private variables

    /// <summary>
    /// The rigidbody attached to our character
    /// </summary>
    private Rigidbody rb = null;

    /// <summary>
    /// The capsule collider attached to our character
    /// </summary>
    private CapsuleCollider capCollider = null;

    /// <summary>
    /// Game properties
    /// </summary>
    private bool hasWon = false;

    // Player properties

    /// <summary>
    /// Tells us if our character is on the ground or not (this is also needed because rigidbody does not have this)
    /// </summary>
    private bool isGrounded = true;

    // Some calculation variables

    /// <summary>
    /// The velocity of the character without accounting for its rotation
    /// </summary>
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// The velocity of the character, accounting for its rotation
    /// </summary>
    private Vector3 localVelocity = Vector3.zero;

    /// <summary>
    /// The bottom of the player's capsule collider
    /// </summary>
    private Vector3 colliderBottom = Vector3.zero;

    /// <summary>
    /// Saves a portion fo the calculation for the collider offset so we don't have to recalculate on upate
    /// (but this will break if we dynamically adjust height/radius/offset properties in the collider)
    /// </summary>
    private Vector3 _colliderBottomOffset = Vector3.zero;

    /// <summary>
    /// Current x-rotation of the camera
    /// </summary>
    private float currentTilt = 0;

    // Public methods

    /// <summary>
    /// Process damage to character (if character has an invulnerability trait, negate the damage... but this isn't implemented yet)
    /// </summary>
    /// <param name="damage">The amount of damage we want to deal to the player</param>
    public void Damage(int damage)
    {
        // Update health to reflect damage
        health -= damage;
        healthText.text = "Health: " + health;

        // If player has <= 0 health, player loses control
        if (health <= 0 && !hasWon)
        {
            // Detach children and destroy this game object
            this.transform.DetachChildren();
            Destroy(this.gameObject);

            // Show loss message
            string lossText = "You Lose... =(";
            endingText.text = lossText;
        }
    }

    // Private methods

    /// <summary>
    /// Handle horizontal movements
    /// </summary>
    /// <param name="forwardBackward">1 for forward and -1 for backward</param>
    /// <param name="leftRight">1 for right and -1 for left</param>
    private void MoveHorizontally(float forwardBackward, float leftRight)
    {
        // Move forward
        if (forwardBackward > 0)
        {
            velocity.z = forwardBackward * (isGrounded ? forwardVelocity : forwardAerialVelocity);
        }
        // Move backward
        else if (forwardBackward < 0)
        {
            velocity.z = forwardBackward * (isGrounded ? backwardVelocity : backwardAerialVelocity);
        }

        // Strafe left (rotate left)
        if (leftRight < 0)
        {
            velocity.x = leftRight * (isGrounded ? leftStrafeVelocity : leftStrafeAerialVelocity);
        }
        // Strafe right (rotate right)
        else if (leftRight > 0)
        {
            velocity.x = leftRight * (isGrounded ? rightStrafeVelocity : rightStrafeAerialVelocity);
        }
    }

    /// <summary>
    /// Make the player jump
    /// </summary>
    private void Jump()
    {
        velocity.y = jumpVelocity;
    }

    /// <summary>
    /// Handle rotation of the camera
    /// </summary>
    private void RotateCamera()
    {
        this.transform.Rotate(0, Input.GetAxis("Mouse X") * cameraSensitivityX, 0);
        currentTilt = Mathf.Clamp(currentTilt - Input.GetAxis("Mouse Y") * cameraSensitivityY, minTilt, maxTilt);
        cameraTarget.localEulerAngles = new Vector3(currentTilt, 0, 0);
    }

    /// <summary>
    /// Freeze rotation on our Rigidbody so our character doesn't fall over
    /// and initialize some variables
    /// </summary>
    private void Awake()
    {
        capCollider = this.GetComponent<CapsuleCollider>();
        // Set the offset for the colliderBottom calculation (more info within the update() function)
        _colliderBottomOffset = capCollider.center + (Vector3.down * ((capCollider.height / 2) - capCollider.radius));
        rb = this.GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        // Configure object to ignore raycasting so when we check for the ground under the player's feet, we don't 
        // account for the feet being the ground
        this.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }

    /// <summary>
    /// Handle player movement
    /// </summary>
    private void Update()
    {
        // Set y velocity to `rb`'s existing y velocity to preserve gravity from previous update
        velocity.Set(0, rb.velocity.y, 0);

        // Get the bottom of the player's capsule collider by getting:
        // (center of the player) + (any offset from the center) + ((0, -1, 0) * ((half capsule height) - (radius of spherical end)))
        // we subtract the radius of the spherical end because we're going to use a sphere with the same radius to detect collisions
        colliderBottom = this.transform.position + _colliderBottomOffset;

        // Simulates a sphere right below the player and returns true if it touches anything, such as the ground
        // Note: I know this makes the check a little further out from the body, but this helps some of the cases where my player won't move and is not noticeable (at least to me)
        isGrounded = Physics.CheckSphere(colliderBottom + (Vector3.down * 0.1f), capCollider.radius);

        if (isGrounded)
        {
            // Jump
            if (Input.GetButton(jumpInput))
            {
                Jump();
            }
        }

        // Handle player's horizontal movement
        float forwardBackInput = Input.GetAxis(moveForwardBackwardInput);
        float leftRightInput = Input.GetAxis(moveLeftRightInput);
        MoveHorizontally(forwardBackInput, leftRightInput);

        // Adjust player's velocity to be an offset based on the player's rotation
        localVelocity = this.transform.TransformDirection(velocity);

        // Apply velocity to `rb`
        rb.velocity = localVelocity;

        // Handle player's camera rotation
        RotateCamera();
    }

    /// <summary>
    /// Upon entering a trigger, either win the game when on a goal trigger
    /// </summary>
    /// <param name="other">The collider we triggered</param>
    private void OnTriggerEnter(Collider other)
    {
        // If triggering "GoalPlatform", let the player know they won
        if (other.tag == "GoalPlatform")
        {
            hasWon = true;
            endingText.text = "You Win!";
        }
    }
}
