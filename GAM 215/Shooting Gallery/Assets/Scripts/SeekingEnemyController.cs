using UnityEngine;
using System.Collections;

/// <summary>
/// Handles AI and event handling for "seeking" enemies
/// </summary>
public class SeekingEnemyController : MonoBehaviour {
    /// <summary>
    /// Reference to player's transform
    /// </summary>
    private Transform playerTransform = null;

    /// <summary>
    /// Reference to our rigidbody
    /// </summary>
    private Rigidbody enemyRigidbody = null;

    /// <summary>
    /// Reference to our material (for getting the color)
    /// </summary>
    private Material myMaterial = null;

    [Header("--- Enemy Config ---")]

    /// <summary>
    /// Our health value
    /// </summary>
    [SerializeField] private int health = 3;

    /// <summary>
    /// Our movement speed
    /// </summary>
    [SerializeField] private float speed = 1.5f;

    /// <summary>
    /// The color we should flash when being damaged
    /// </summary>
    [SerializeField] private Color flashColor = Color.red;

    /// <summary>
    /// Calculation variable for our velocity (relative to where we are facing)
    /// </summary>
    private Vector3 localVelocity = Vector3.zero;

    /// <summary>
    /// Our original color
    /// </summary>
    private Color originalColor;

    /// <summary>
    /// Modify the health of enemy
    /// </summary>
    /// <param name="value">The value to modify the health with</param>
    public void ChangeHealth(int value)
    {
        // TODO: Apply some damage reduction?
        health += value;

        // Destroy enemy when health goes below 0
        if (health <= 0)
        {
            this.transform.DetachChildren();
            Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Apply a new color to enemy
    /// </summary>
    /// <param name="color">The color we want to apply</param>
    public void ChangeColor(Color color)
    {
        myMaterial.color = color;
    }

	/// <summary>
    /// Initialize references
    /// </summary>
	private void Awake()
    {
        // There should only be one player, but if we ever make this a multiplayer game, then we might want to reconsider this method of initialization
        playerTransform = GameObject.FindWithTag("Player").transform;
        enemyRigidbody = this.GetComponent<Rigidbody>();

        // Initialize our original color
        myMaterial = this.GetComponent<MeshRenderer>().material;
        originalColor = myMaterial.color;
	}
	
	/// <summary>
    /// Handle movements every frame
    /// </summary>
	private void Update()
    {
        // Look at player
        this.transform.LookAt(playerTransform);

        // Move forward relative to where enemy is facing
        localVelocity = this.transform.TransformDirection(Vector3.forward * speed);

        // Apply velocity
        enemyRigidbody.velocity = localVelocity;
	}

    /// <summary>
    /// Handle damage and color change on mouse down
    /// </summary>
    private void OnMouseDown()
    {
        ChangeColor(flashColor);

        // TODO: Get this value from the player depending on player's weapon/attack value
        ChangeHealth(-1);
    }

    /// <summary>
    /// Return color back to original color on mouse up
    /// </summary>
    private void OnMouseUp()
    {
        ChangeColor(originalColor);
    }
}
