using UnityEngine;
using System.Collections;

/// <summary>
/// Handles game logic for player events
/// </summary>
public class PlayerController : MonoBehaviour {
    #region Reference Variables

    /// <summary>
    /// Our rigidbody
    /// </summary>
    private Rigidbody playerRigidbody = null;

    /// <summary>
    /// The game object to spawn when we fire a projectile
    /// </summary>
    [SerializeField] private GameObject projectile = null;

    /// <summary>
    /// The game controller
    /// </summary>
    private GameController gameController;

    #endregion

    #region Player Config

    /// <summary>
    /// Max health of the player
    /// </summary>
    [SerializeField] private int maxHealth = 3;

    /// <summary>
    /// The speed of the player
    /// </summary>
    [SerializeField] private float speed = 5.0f;

    /// <summary>
    /// The firing delay of the weapon in seconds
    /// </summary>
    [SerializeField] private float firingDelay = 0.25f;

    /// <summary>
    /// The amount of damage the player should do
    /// </summary>
    [SerializeField] private int damage = 1;

    #endregion

    #region Input Axis Config

    /// <summary>
    /// Input axis name for the horizontal movement
    /// </summary>
    [SerializeField] private string horizontalMovementInputName = "Horizontal";

    /// <summary>
    /// Input axis name for the vertical movement
    /// </summary>
    [SerializeField] private string verticalMovementInputName = "Vertical";

    /// <summary>
    /// Input axis name for firing a weapon
    /// </summary>
    [SerializeField] private string firingInputName = "Fire1";

    #endregion

    #region Calculation Variables

    /// <summary>
    /// The velocity we want to apply to our rigidbody
    /// </summary>
    private Vector3 velocity = Vector3.zero;

    /// <summary>
    /// Indicates the last time the player fired their weapon
    /// </summary>
    private float lastFired = 0;

    /// <summary>
    /// Ray we'll use when firing
    /// </summary>
    private Ray firingRay;

    /// <summary>
    /// The position we are firing towards
    /// </summary>
    private Vector3 targetPoint = Vector3.zero;

    /// <summary>
    /// The rotation of the projectile
    /// </summary>
    private Quaternion targetRotation = Quaternion.identity;

    #endregion

    #region Helper Functions

    /// <summary>
    /// The logic for moving the player
    /// </summary>
    /// <param name="horizontalValue">The amount we move left/right</param>
    /// <param name="verticalValue">The amount we move up/down</param>
    private void Move(float horizontalValue, float verticalValue)
    {
        // Apply the horizontal and vertical movement with the player's speed
        velocity.x = horizontalValue * speed;
        velocity.y = 0;
        velocity.z = verticalValue * speed;

        // Actually apply the velocity to the player's rigidbody
        playerRigidbody.velocity = velocity;
    }

    /// <summary>
    /// Logic for where to shoot the projectile and how
    /// </summary>
    private void FireWeapon()
    {
        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Generate a ray from the cursor position
        firingRay = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        // This will be the point that the object must look towards to be looking at the mouse.
        // Raycasting to a Plane object only gives us a distance, so we'll have to take the distance,
        //   then find the point along that ray that meets that distance.  This will be the point
        //   to look at.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(firingRay, out hitdist))
        {
            // Get the point along the ray that hits the calculated distance.
            targetPoint = firingRay.GetPoint(hitdist);

            // Determine the target rotation.  This is the rotation if the transform looks at the target point.
            targetRotation = Quaternion.LookRotation(targetPoint - transform.position);

            // Create bullet and configure it
            gameController.SpawnProjectile(this.transform.position, targetRotation, this.tag, "Enemy", damage);
        }
    }

#endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        playerRigidbody = this.GetComponent<Rigidbody>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
	}
	
	/// <summary>
    /// Handle events every frame
    /// </summary>
	private void Update()
    {
        // Handle player movement
        float horizontalValue = Input.GetAxis(horizontalMovementInputName);
        float verticalValue = Input.GetAxis(verticalMovementInputName);
        Move(horizontalValue, verticalValue);

        // Trigger weapon fire if input is pressed and the weapon is not on cooldown
        float now = Time.time;
        if (Input.GetAxis(firingInputName) != 0 && now > lastFired + firingDelay)
        {
            lastFired = now;
            FireWeapon();
        }
	}

#endregion
}
