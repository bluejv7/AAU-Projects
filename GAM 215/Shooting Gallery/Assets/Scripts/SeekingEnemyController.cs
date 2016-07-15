using UnityEngine;
using System.Collections;

/// <summary>
/// Handles AI and event handling for "seeking" enemies
/// (I probably should have inherited from game controller, but I'm not sure how I should use inheritance with Unity)
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

    /// <summary>
    /// Reference to the game controller
    /// </summary>
    private GameController gameController = null;

    [Header("--- Enemy Config ---")]

    /// <summary>
    /// Our max health
    /// </summary>
    [SerializeField] private int maxHealth = 3;

    /// <summary>
    /// Our movement speed
    /// </summary>
    [SerializeField] private float speed = 3.5f;

    /// <summary>
    /// The amount of damage we do
    /// </summary>
    [SerializeField] private int damage = 1;

    /// <summary>
    /// The delay between attacks in seconds
    /// </summary>
    [SerializeField] private float attackDelay = 1000.0f;

    /// <summary>
    /// Our current health
    /// </summary>
    private int health = 3;

    /// <summary>
    /// Calculation variable for our velocity (relative to where we are facing)
    /// </summary>
    private Vector3 localVelocity = Vector3.zero;

    /// <summary>
    /// Our current color
    /// </summary>
    private Color currentColor;

    /// <summary>
    /// Our original color
    /// </summary>
    private Color originalColor;

    /// <summary>
    /// The time of the last attack made
    /// </summary>
    private float lastAttackTime = 0;

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
        gameController = GameObject.FindWithTag("GameController").GetComponent<GameController>();

        // Initialize our original color
        myMaterial = this.GetComponent<MeshRenderer>().material;
        originalColor = myMaterial.color;
        currentColor = originalColor;
	}
	
	/// <summary>
    /// Handle movements every frame
    /// </summary>
	private void Update()
    {
        // If player is dead, don't try to track it anymore
        if (!playerTransform)
        {
            return;
        }

        // Look at player
        this.transform.LookAt(playerTransform);

        // Move forward relative to where enemy is facing
        localVelocity = this.transform.TransformDirection(Vector3.forward * speed);

        // Apply velocity
        enemyRigidbody.velocity = localVelocity;
	}

    /// <summary>
    /// Handles collision event every frame it is colliding
    /// </summary>
    /// <param name="other">The object it is colliding with</param>
    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.tag == "Base")
        {
            // If the time elapsed is greater than the attack delay, attack the base
            float now = Time.time;
            if (now - lastAttackTime > attackDelay)
            {
                lastAttackTime = now;
                gameController.DamageBase(damage);
            }
        }
    }

    /// <summary>
    /// Event when we are shot
    /// </summary>
    /// <param name="damage">The damage to apply</param>
    private void OnShot(int damage)
    {
        // Damage is a subtraction of health, so we need to use the negative of damage to apply to health
        // (might be neat to apply healing damage for elemental weapons used on same elemental monsters)
        ChangeHealth(-damage);

        // Go from white to more red as the enemy gets more damaged
        currentColor.b = originalColor.b * health / maxHealth;
        currentColor.g = originalColor.g * health / maxHealth;
        ChangeColor(currentColor);
    }
}
