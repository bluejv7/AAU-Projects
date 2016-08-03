using UnityEngine;
using System.Collections;

/// <summary>
/// Handle movement and events for a shooting enemy
/// </summary>
public class ShootingEnemyController : MonoBehaviour
{
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
    /// The game controller we want to make requests to
    /// </summary>
    private GameController gameController = null;

    #endregion

    #region Enemy Configs

    /// <summary>
    /// The max distance required to start following the player
    /// </summary>
    [SerializeField] private float visibilityDistance = 15.0f;

    /// <summary>
    /// The distance a visible player has to be before we start running
    /// </summary>
    [SerializeField] private float initialFleeDistance = 10.0f;

    /// <summary>
    /// Our speed
    /// </summary>
    [SerializeField] private float speed = 5.0f;

    /// <summary>
    /// Our damage towards the player
    /// </summary>
    [SerializeField] private int damage = 1;

    /// <summary>
    /// The time between attacks
    /// </summary>
    [SerializeField] private float attackDelay = 1.0f;

    /// <summary>
    /// Our max health
    /// </summary>
    [SerializeField] private int maxHealth = 3;

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
    /// Layer for enemy
    /// </summary>
    private int enemyLayer = 0;

    /// <summary>
    /// The calculated layerMask to send for raycasts
    /// </summary>
    private int layerMask = 0;

    /// <summary>
    /// The time when we last attacked
    /// </summary>
    private float lastAttackTime = 0;

    #endregion

    #region Helper Functions

    /// <summary>
    /// Look at and shoot at player
    /// </summary>
    private void ShootAtPlayer()
    {
        this.transform.LookAt(player.transform);

        // If our attack cooldown finished, fire a projectile at the player
        float now = Time.time;
        if (now >= lastAttackTime + attackDelay)
        {
            gameController.SpawnProjectile(this.transform.position, this.transform.rotation, this.tag, player.tag, damage);
            lastAttackTime = now;
        }
    }

    /// <summary>
    /// Run away from the player
    /// </summary>
    private void RunFromPlayer()
    {
        localVelocity = this.transform.TransformDirection(Vector3.back * speed);
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
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();

        // Initialize `enemyRenderer` variable based on the enemy
        InitRenderer();

        originalColor = enemyRenderer.material.color;
        currentColor = originalColor;
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
            return;
        }

        // Check if player is within visibility distance using raycast
        ray = new Ray(this.transform.position, player.transform.position - this.transform.position);
        if (Physics.Raycast(ray, out hitInfo, visibilityDistance, layerMask))
        {
            bool isPlayer = hitInfo.collider.tag == "Player";
            // If raycast hits player, shoot at player
            if (isPlayer)
            {
                ShootAtPlayer();
            }

            // If player is too close to us, move backwards
            if (isPlayer && hitInfo.distance < initialFleeDistance)
            {
                RunFromPlayer();
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
