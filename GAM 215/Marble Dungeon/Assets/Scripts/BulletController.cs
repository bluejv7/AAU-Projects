using UnityEngine;
using System.Collections;

/// <summary>
/// Logic for handling events and movement for the bullet
/// </summary>
public class BulletController : MonoBehaviour {
    #region Reference Variables

    /// <summary>
    /// Reference variable for our rigidbody
    /// </summary>
    private Rigidbody bulletRigidbody = null;

    /// <summary>
    /// Reference variable for the game controller
    /// </summary>
    [SerializeField] private GameController gameController = null;

    #endregion

    #region Bullet Configs

    /// <summary>
    /// The speed of the bullet
    /// </summary>
    [SerializeField] private float speed = 5.0f;

    /// <summary>
    /// The X boundaries for the bullet
    /// </summary>
    [SerializeField] private float boundaryX = 50.0f;

    /// <summary>
    /// The Z boundaries for the bullet
    /// </summary>
    [SerializeField] private float boundaryZ = 50.0f;

    /// <summary>
    /// The amount of damage the bullet will inflict
    /// </summary>
    public int damage = 1;

    /// <summary>
    /// Indicates whether this bullet pierces through enemies
    /// </summary>
    public bool isPiercing = false;

    #endregion

    #region Calculation variables

    /// <summary>
    /// Calculation variable for the local velocity
    /// </summary>
    private Vector3 localVelocity = Vector3.zero;

    /// <summary>
    /// The position where bullet was spawned
    /// </summary>
    private Vector3 spawnPoint = Vector3.zero;

    #endregion

    #region Public Variables

    /// <summary>
    /// GameObject tag to damage
    /// </summary>
    public string damageTag = "";

    /// <summary>
    /// The tag of the object that spawn this bullet
    /// </summary>
    public string spawnTag = "";

    /// <summary>
    /// Allow bullets to collide with themselvess
    /// </summary>
    public bool collideWithSelf = true;

    #endregion

    #region Helper Functions

    /// <summary>
    /// Check if bullet has passed its allowed boundaries
    /// </summary>
    /// <returns></returns>
    private bool isPastBoundary()
    {
        if (Mathf.Abs(this.transform.position.x - spawnPoint.x) > boundaryX || Mathf.Abs(this.transform.position.z - spawnPoint.z) > boundaryZ)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Function for initializing the object after it is enabled
    /// (Note: It only has one statement right now, but this is just in case we want to initialize other things that should happen more than once)
    /// </summary>
    public void initialize()
    {
        spawnPoint = this.transform.position;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        bulletRigidbody = this.GetComponent<Rigidbody>();

        // Initialize other properties that may need to be re-initialized later on
        initialize();
    }

	/// <summary>
    /// Handle movement every frame and out of bounds calls
    /// </summary>
	private void Update()
    {
        // Handle movement
        localVelocity = this.transform.TransformDirection(Vector3.forward * speed);
        bulletRigidbody.velocity = localVelocity;

        // Return bullet to projectile pool when out of bounds
        if (isPastBoundary())
        {
            gameController.PushPool(this.gameObject);
        }
    }

    /// <summary>
    /// Handle collisions for the bullet
    /// </summary>
    /// <param name="other">The object we're triggering or the object that triggered us</param>
    private void OnTriggerEnter(Collider other)
    {
        // Send "OnShot" message if the bullet collides with the correct object
        string tag = other.gameObject.tag;
        if (tag == damageTag)
        {
            other.gameObject.SendMessage("OnShot", damage, SendMessageOptions.DontRequireReceiver);
        }

        // Don't destroy bullet if it is a piercing type and it doesn't hit a wall
        if (isPiercing && tag != "Wall")
        {
            return;
        }

        // Don't destroy bullet if the trigger is the one that spawned it
        if (tag == spawnTag)
        {
            return;
        }

        // If we don't allow it, bullets won't destroy themselves if they're spawned from the same thing
        if (tag == this.tag && spawnTag == other.gameObject.GetComponent<BulletController>().spawnTag && !collideWithSelf)
        {
            return;
        }

        // Otherwise, return bullet to the projectile pool
        gameController.PushPool(this.gameObject);
    }

    #endregion
}
