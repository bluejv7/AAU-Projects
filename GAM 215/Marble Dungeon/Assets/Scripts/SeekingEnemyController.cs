using UnityEngine;
using System.Collections;

/// <summary>
/// Handle movement and events for a seek and destroy enemy
/// </summary>
public class SeekingEnemyController : MonoBehaviour {
    #region Reference Variables

    /// <summary>
    /// Reference variable for the player
    /// </summary>
    private GameObject player = null;

    /// <summary>
    /// Our rigidbody component
    /// </summary>
    private Rigidbody enemyRigidbody = null;

    #endregion

    #region Enemy Configs

    /// <summary>
    /// The max distance required to start following the player
    /// </summary>
    [SerializeField] private float visibilityDistance = 10.0f;

    /// <summary>
    /// Our speed
    /// </summary>
    [SerializeField] private float speed = 5.0f;

    /// <summary>
    /// Our damage towards the player
    /// </summary>
    [SerializeField] private int damage = 1;

    #endregion

    #region Calculation Variables

    /// <summary>
    /// Ray to cast to determine visibility
    /// </summary>
    private Ray ray;

    /// <summary>
    /// Hit information for the raycast
    /// </summary>
    private RaycastHit hitInfo;

    /// <summary>
    /// The velocity relative to our transform
    /// </summary>
    private Vector3 localVelocity = Vector3.zero;

    #endregion

    #region Helper Functions

    /// <summary>
    /// Look at and move towards player
    /// </summary>
    private void FollowPlayer()
    {
        this.transform.LookAt(player.transform);
        localVelocity = this.transform.TransformDirection(Vector3.forward * speed);
        enemyRigidbody.velocity = localVelocity;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyRigidbody = this.GetComponent<Rigidbody>();
	}
	
	/// <summary>
    /// Handle movement every frame
    /// </summary>
	private void Update()
    {
        // If player died, don't look check for player
        if (!player)
        {
            return;
        }

        // Check if player is within visibility distance using raycast
        ray = new Ray(this.transform.position, player.transform.position - this.transform.position);
        if (Physics.Raycast(ray, out hitInfo, visibilityDistance))
        {
            // If raycast hits player, follow player
            if (hitInfo.collider.tag == "Player")
            {
                FollowPlayer();
            }
        }
	}

    /// <summary>
    /// Decide what to do when colliding with another collider
    /// </summary>
    /// <param name="other">The object we are colliding with</param>
    private void OnCollisionStay(Collision other)
    {
        // If we collided with the player, try to damage player
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.SendMessage("OnHit", damage);
        }
    }

    #endregion
}
