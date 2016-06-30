using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Controller for main character (using `Rigidbody`)
/// </summary>
public class CharacterRigidBodyController : MonoBehaviour
{
    // public variables

    /// <summary>
    /// The text `GameObject` on the canvas
    /// </summary>
    public Text healthText;

    /// <summary>
    /// The text `GameObject` to indicate win/loss condition
    /// </summary>
    public Text endingText;

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
    /// The health of our player
    /// </summary>
    private int health = 100;

    /// <summary>
    /// Indicates whether the player can control their character or not
    /// </summary>
    private bool hasControl = true;

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


    // Movement properties

    /// <summary>
    /// The velocity we will use when moving forward
    /// </summary>
    private float forwardVelocity = 4.0f;

    /// <summary>
    /// The velocity we will use when moving forward in the air
    /// </summary>
    private float forwardAerialVelocity = 2.5f;

    /// <summary>
    /// The velocity we will use when moving backwards
    /// </summary>
    private float backwardVelocity = -4.0f;

    /// <summary>
    /// The velocity we will use when moving backwards in the air
    /// </summary>
    private float backwardAerialVelocity = -2.5f;

    /// <summary>
    /// The amount of degrees to turn left
    /// </summary>
    private float leftTurnDegrees = -180.0f;

    /// <summary>
    /// The amount of degrees to turn right
    /// </summary>
    private float rightTurnDegrees = 180.0f;

    /// <summary>
    /// The jump velocity
    /// </summary>
    private float jumpVelocity = 4.0f;

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
        // we subtract the radius of the spherical end because we're going to use a sphere with the same radius to detect collisions (is that right?)
        colliderBottom = this.transform.position + _colliderBottomOffset;

        // Simulates a sphere right below the player and returns true if it touches anything, such as the ground
        // Note: I know this makes the check a little further out from the body, but this helps some of the cases where my player won't move and is not noticeable (at least to me)
        isGrounded = Physics.CheckSphere(colliderBottom + (Vector3.down * 0.1f), capCollider.radius);

        if (isGrounded)
        {
            // Jump
            if (Input.GetKey(KeyCode.Space))
            {
                velocity.y = jumpVelocity;
            }
        }

        // Move forward
        if (Input.GetKey(KeyCode.W))
        {
            velocity.z = (isGrounded ? forwardVelocity : forwardAerialVelocity);
        }
        // Move backward
        else if (Input.GetKey(KeyCode.S))
        {
            velocity.z = (isGrounded ? backwardVelocity : backwardAerialVelocity);
        }

        // Look left (rotate left)
        if (Input.GetKey(KeyCode.A))
        {
            this.transform.Rotate(0, leftTurnDegrees * Time.deltaTime, 0);
        }
        // Look right (rotate right)
        else if (Input.GetKey(KeyCode.D))
        {
            this.transform.Rotate(0, rightTurnDegrees * Time.deltaTime, 0);
        }


        // Adjust player's velocity to be an offset based on the player's rotation
        localVelocity = this.transform.TransformDirection(velocity);

        // Apply velocity to `rb`
        rb.velocity = localVelocity;
    }

    /// <summary>
    /// Temporarily define damage here until I learn how to grab it from the enemy/hazard's property
    /// ... I'm sure there's a way to do that, right?
    /// </summary>
    private int damage = 99;

    /// <summary>
    /// Upon entering a trigger, either win the game when on a goal trigger or take damage from other triggers
    /// </summary>
    private void OnTriggerEnter(Collider other)
    {
        // If triggering "GoalPlatform", let the player know they won
        if (other.tag == "GoalPlatform")
        {
            hasWon = true;
            endingText.text = "You Win!";
        }
        // Otherwise, player takes damage and update health value
        else
        {
            health -= damage;
            healthText.text = "Health: " + health;
        }

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
}
