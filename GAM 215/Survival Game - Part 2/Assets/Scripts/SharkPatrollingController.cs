using UnityEngine;
using System.Collections;

/// <summary>
/// The controller/AI for a patrolling shark
/// </summary>
public class SharkPatrollingController : MonoBehaviour
{
    // Serialized private variables 

    /// <summary>
    /// The amount of frames shark should be bobbing for
    /// </summary>
    [SerializeField] private int bobDuration = 45;

    /// <summary>
    /// The speed at which the shark bobs up and down
    /// </summary>
    [SerializeField] private float bobVelocity = 0.25f;

    /// <summary>
    /// The patrol positions for the shark to rotate between
    /// </summary>
    [SerializeField] private Vector3[] patrolPositions;

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

    /// <summary>
    /// The index of which patrol position we're heading towards
    /// </summary>
    private int currentPatrolIndex = 0;

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

    // Private methods

    /// <summary>
    /// Handle initializing variables
    /// </summary>
    private void Awake()
    {
        rb = this.GetComponent<Rigidbody>();

        // Get rotation in euler angles (referenced documentation here: http://docs.unity3d.com/ScriptReference/Quaternion-eulerAngles.html)
        originalRotationZ = rb.rotation.eulerAngles.z;

        // Ignore collision for the plane, because it's a terrain shark =)
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Plane"), LayerMask.NameToLayer("SharkPatrol"), true);
        // Also have these ignore each other
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("SharkPatrol"), LayerMask.NameToLayer("SharkPatrol"), true);

        // Base min/max Y positions off of (current Y) - (bob velocity)
        maxPosY = this.transform.position.y + bobVelocity;
        minPosY = this.transform.position.y - bobVelocity;
    }

    /// <summary>
    /// Handle simple AI patrol behavior and bobbing movement
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

        // Look at current patrol position
        // Note: Is there a better way to set this without setting it equal to "new Vector3(float, float, float)"?  I don't want to create and destroy during updates if possible
        targetPosition.x = patrolPositions[currentPatrolIndex].x;
        targetPosition.y = this.transform.position.y;
        targetPosition.z = patrolPositions[currentPatrolIndex].z;
        this.transform.LookAt(targetPosition);

        // A bit of a hack to reset the model to the rotation I want it at
        this.transform.Rotate(0, rotateOffsetY, originalRotationZ);

        // If we reached our patrol position, set the patrol index to the next position
        if (Mathf.Abs(this.transform.position.x - patrolPositions[currentPatrolIndex].x) < 0.05f && Mathf.Abs(this.transform.position.z - patrolPositions[currentPatrolIndex].z) < 0.05f)
        {
            // Get new patrol index
            currentPatrolIndex = (currentPatrolIndex + 1) % patrolPositions.Length;
        }
        // otherwise, just move forward
        else
        {
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
