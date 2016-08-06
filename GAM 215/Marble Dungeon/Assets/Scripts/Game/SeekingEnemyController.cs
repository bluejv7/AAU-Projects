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

    /// <summary>
    /// Our renderer component
    /// </summary>
    private Renderer enemyRenderer = null;

    /// <summary>
    /// Our animator component
    /// </summary>
    private Animator enemyAnimator = null;

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

    /// <summary>
    /// Our max health
    /// </summary>
    [SerializeField] private int maxHealth = 4;

    /// <summary>
    /// Sound to play when attacking
    /// </summary>
    [SerializeField] private AudioClip attackSound = null;

    /// <summary>
    /// Sound to play when getting damaged
    /// </summary>
    [SerializeField] private AudioClip damageSound = null;

    /// <summary>
    /// The layer name for the projectile
    /// </summary>
    [SerializeField] private string projectileLayerName = "Projectile";

    /// <summary>
    /// Layer name for enemy
    /// </summary>
    [SerializeField] private string enemyLayerName = "Enemy";

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

    /// <summary>
    /// Our current health
    /// </summary>
    private int currentHealth = 0;

    /// <summary>
    /// The original color of our enemy
    /// </summary>
    private Color originalColor;

    /// <summary>
    /// Enemy's current color (not an actual current color... just a variable instantiated here so we don't have to instantiate it every time enemy is hit)
    /// </summary>
    private Color currentColor;

    /// <summary>
    /// The layer of the projectile
    /// </summary>
    private int projectileLayer = 0;


    /// <summary>
    /// Layer of enemy
    /// </summary>
    private int enemyLayer = 0;

    /// <summary>
    /// The calculated layerMask to send for raycasts
    /// </summary>
    private int layerMask = 0;

    /// <summary>
    /// Determine whether we should use the raycast for vision
    /// </summary>
    private bool limitedVision = true;

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

    /// <summary>
    /// Handle changes to our current health
    /// </summary>
    /// <param name="value">The amount we want to change our health</param>
    private void ChangeHealth(int value)
    {
        currentHealth += value;

        // If we has 0 or less health, destroy ourselves
        if (currentHealth <= 0)
        {
            foreach (Transform child in this.transform)
            {
                GameObject.Destroy(child.gameObject);
            }
            this.transform.DetachChildren();
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Change the color of the enemy
    /// </summary>
    private void ChangeColor()
    {
        currentColor.g = originalColor.g * currentHealth / maxHealth;
        currentColor.b = originalColor.b * currentHealth / maxHealth;
        enemyRenderer.material.color = currentColor;
    }

    /// <summary>
    /// Handle rendering of different enemies
    /// </summary>
    private void InitRenderer()
    {
        enemyRenderer = this.GetComponent<Renderer>();

        // If we can't grab the renderer from our object, look in the children
        if (!enemyRenderer)
        {
            enemyRenderer = this.GetComponentInChildren<Renderer>();
        }
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

        // Initialize `enemyRenderer` variable based on the enemy
        InitRenderer();

        originalColor = enemyRenderer.material.color;
        currentColor = originalColor;
        enemyAnimator = this.GetComponent<Animator>();
        currentHealth = maxHealth;

        // Get the projectile layer, bitshift to get the projectile layer and enemy as the mask, and then invert the mask so we raycast past the projectile
        // Got help from (http://answers.unity3d.com/questions/55829/help-with-ignoring-collider-with-raycast.html)
        // Also got help with (http://answers.unity3d.com/questions/416919/making-raycast-ignore-multiple-layers.html) for ignoring multiple layers
        projectileLayer = LayerMask.NameToLayer(projectileLayerName);
        enemyLayer = LayerMask.NameToLayer(enemyLayerName);
        layerMask = 1 << projectileLayer;
        layerMask |= (1 << enemyLayer);
        layerMask = ~layerMask;
	}
	
	/// <summary>
    /// Handle movement every frame
    /// </summary>
	private void Update()
    {
        // If player died, don't look check for player, stop moving, and stop attacking
        if (!player)
        {
            enemyRigidbody.velocity = Vector3.zero;
            if (enemyAnimator)
            {
                enemyAnimator.SetBool("idle0ToAttack1", false);
            }
            return;
        }

        // If we no longer have limited vision, just follow the player
        if (!limitedVision)
        {
            FollowPlayer();
            return;
        }

        // Check if player is within visibility distance using raycast
        ray = new Ray(this.transform.position, player.transform.position - this.transform.position);
        if (Physics.Raycast(ray, out hitInfo, visibilityDistance, layerMask))
        {
            // If raycast hits player, follow player
            if (hitInfo.collider.tag == "Player")
            {
                limitedVision = false;
                FollowPlayer();
                return;
            }
        }

        // Otherwise, don't move
        enemyRigidbody.velocity = Vector3.zero;
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

            // Animate attack
            if (enemyAnimator)
            {
                enemyAnimator.SetBool("idle0ToAttack1", true);
            }

            // Play attack sound
            AudioSource.PlayClipAtPoint(attackSound, this.transform.position);
        }
    }

    /// <summary>
    /// Decide what to do when leaving collision with something
    /// </summary>
    /// <param name="other">The object we are no longer colliding with</param>
    private void OnCollisionExit(Collision other)
    {
        // If we aren't in range of player, don't animate attack anymore
        if (other.gameObject.tag == "Player")
        {
            if (enemyAnimator)
            {
                enemyAnimator.SetBool("idle0ToAttack1", false);
            }
        }
    }

    /// <summary>
    /// Handle what happens when we get shot
    /// </summary>
    /// <param name="damage">The amount of damage we might take</param>
    private void OnShot(int damage)
    {
        // Try to hurt the enemy and change color accordingly
        ChangeHealth(-damage);
        ChangeColor();

        // Play damage sound
        AudioSource.PlayClipAtPoint(damageSound, this.transform.position);
    }

    #endregion
}
