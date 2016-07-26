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

    #endregion

    #region Bullet Configs

    /// <summary>
    /// The speed of the bullet
    /// </summary>
    [SerializeField] private float speed = 5.0f;

    /// <summary>
    /// The X boundaries for the bullet
    /// </summary>
    [SerializeField] private float boundaryX = 50f;

    /// <summary>
    /// The Z boundaries for the bullet
    /// </summary>
    [SerializeField] private float boundaryZ = 50f;

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

    #endregion

    #region Helper Functions

    /// <summary>
    /// Check if bullet has passed its allowed boundaries
    /// </summary>
    /// <returns></returns>
    private bool isPastBoundary()
    {
        if (Mathf.Abs(this.transform.position.x) > boundaryX || Mathf.Abs(this.transform.position.z) > boundaryZ)
        {
            return true;
        }

        return false;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        bulletRigidbody = this.GetComponent<Rigidbody>();
        spawnPoint = this.transform.position;
    }

	/// <summary>
    /// Handle movement every frame and out of bounds calls
    /// </summary>
	private void Update()
    {
        // Handle movement
        localVelocity = this.transform.TransformDirection(Vector3.forward * speed);
        bulletRigidbody.velocity = localVelocity;

        // Destroy when out of bounds
        if (isPastBoundary())
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Handle collisions for the bullet
    /// </summary>
    /// <param name="other"></param>
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

        // Otherwise, destroy the bullet
        GameObject.Destroy(this.gameObject);
    }

    #endregion
}
