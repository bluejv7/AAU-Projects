using UnityEngine;
using System.Collections;

/// <summary>
/// The controller/AI for the shark
/// </summary>
public class SharkController : MonoBehaviour {
    // Public variables

    /// <summary>
    /// The player game object reference
    /// </summary>
    [SerializeField] private GameObject player = null;

    /// <summary>
    /// The amount of frames shark should be bobbing for
    /// </summary>
    [SerializeField] private int bobDuration = 45;

    /// <summary>
    /// The speed at which the shark bobs up and down
    /// </summary>
    [SerializeField] private float bobVelocity = 0.25f;

    /// <summary>
    /// Distance at which shark will follow player
    /// </summary>
    [SerializeField] private float followDistance = 5.0f;

    /// <summary>
    /// Shark speed moving forward
    /// </summary>
    [SerializeField] private float forwardVelocity = 2.5f;

    /// <summary>
    /// The offset rotation for the shark, since the "front" of the shark is wrong because of the model
    /// </summary>
    [SerializeField] private float rotateOffsetY = 90.0f;

    /// <summary>
    /// The amount of damage shark will deal
    /// </summary>
    [SerializeField] private int damage = 20;

    // Private variables

    /// <summary>
    /// Shark's rigidbody reference
    /// </summary>
    private Rigidbody rb = null;

    /// <summary>
    /// Define enumerator for the bob states
    /// </summary>
    private enum BobState { Up, Down, Steady };

    /// <summary>
    /// The bobbing state of shark (like the bobbing up and down of objects in water)
    /// </summary>
    private BobState bobState = BobState.Up;

    /// <summary>
    /// The current number of frames shark has been bobbing
    /// </summary>
    private int currentBobDuration = 0;

    /// <summary>
    /// The max Y position the shark can be at
    /// </summary>
    private float maxPosY = 0.0f;

    /// <summary>
    /// The min Y position the shark can be at
    /// </summary>
    private float minPosY = 0.0f;

    // Some calculation variables

    /// <summary>
    /// The velocity of the shark without accounting for its rotation
    /// </summary>
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// The velocity of the shark, accounting for its rotation
    /// </summary>
    private Vector3 localVelocity = Vector3.zero;

    /// <summary>
    /// The absolute Y speed we want to use for bobbing the shark
    /// </summary>
    private float absoluteVerticalVelocity = 0.0f;

    /// <summary>
    /// The distance away from player (setting this to any value doesn't matter, as we will set the variable before checking each time)
    /// </summary>
    private float distanceFromPlayer = 0.0f;

    /// <summary>
    /// The target position we want to look at
    /// </summary>
    private Vector3 targetPosition = Vector3.zero;

    /// <summary>
    /// Keep track of the original Z rotation (gets initialized in Awake())
    /// </summary>
    private float originalRotationZ = 0.0f;

    /// <summary>
    /// Used to help correct the position of the shark
    /// </summary>
    private Vector3 correctedPosition = Vector3.zero;

    /// <summary>
    /// Handle initializing variables
    /// </summary>
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        // Get rotation in euler angles (referenced documentation here: http://docs.unity3d.com/ScriptReference/Quaternion-eulerAngles.html)
        originalRotationZ = rb.rotation.eulerAngles.z;

        // Ignore collision for the plane, because it's a terrain shark =)
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Plane"), LayerMask.NameToLayer("Shark"), true);

        // Base min/max Y positions off of (current Y) - (bob velocity)
        maxPosY = this.transform.position.y + bobVelocity;
        minPosY = this.transform.position.y - bobVelocity;
    }
	
	/// <summary>
    /// Handle simple AI follow behavior and bobbing movement
    /// </summary>
	private void FixedUpdate()
    {
        // Reset velocity
        velocity = Vector3.zero;

        // Increment currentBobDuration
        currentBobDuration++;

        // Move the shark vertically according to BobState
        if (bobState == BobState.Up)
        {
            absoluteVerticalVelocity = bobVelocity;
        }
        else if (bobState == BobState.Down)
        {
            absoluteVerticalVelocity = -bobVelocity;
        }

        // Check if we should change the bob state and reset the duration
        if (currentBobDuration >= bobDuration && bobState != BobState.Steady)
        {
            bobState = (bobState == BobState.Up ? BobState.Down : BobState.Up);
            currentBobDuration = 0;
        }

        // Move the shark towards the player if player is within follow distance
        distanceFromPlayer = Vector3.Distance(this.transform.position, player.transform.position);
        if (distanceFromPlayer <= followDistance)
        {
            // Try to only rotate around the Y axis (I used this as a reference: http://forum.unity3d.com/threads/lookat-to-rotate-only-on-y-axis.49471/)
            targetPosition.x = player.transform.position.x;
            targetPosition.y = this.transform.position.y;
            targetPosition.z = player.transform.position.z;
            this.transform.LookAt(targetPosition);

            // A bit of a hack to reset the model to the rotation I want it at
            this.transform.Rotate(0, rotateOffsetY, originalRotationZ);

            // Also a hack to make the model move "forward"
            velocity.x = -forwardVelocity;
        }

        // Get velocity based on how the shark is rotated
        localVelocity = this.transform.TransformDirection(velocity);

        // Make sure we apply a direct Y velocity for bobbing
        localVelocity.y = absoluteVerticalVelocity;

        // Hack for correcting Y values when they get messed up during collisions (usually only when player's rb moves the shark's rb)
        if (rb.position.y > maxPosY)
        {
            correctedPosition.x = this.transform.position.x;
            correctedPosition.y = maxPosY;
            correctedPosition.z = this.transform.position.z;
            this.transform.position = correctedPosition;
            localVelocity.y = 0;
        }
        else if (rb.position.y < minPosY)
        {
            correctedPosition.x = this.transform.position.x;
            correctedPosition.y = minPosY;
            correctedPosition.z = this.transform.position.z;
            this.transform.position = correctedPosition;
            localVelocity.y = 0;
        }

        // Apply final velocity calculations
        rb.velocity = localVelocity;
    }

    /// <summary>
    /// Deal damage if we collide with player
    /// </summary>
    /// <param name="other">The collision object we collided with</param>
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            CharacterRigidBodyController playerController = other.gameObject.GetComponent<CharacterRigidBodyController>();
            playerController.Damage(damage);
        }
    }
}
