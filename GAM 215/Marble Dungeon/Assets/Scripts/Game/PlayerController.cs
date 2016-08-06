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
    /// Reference to our renderer componenet
    /// </summary>
    private Renderer playerRenderer = null;

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

    /// <summary>
    /// The amount of seconds that must pass before getting damaged again
    /// </summary>
    [SerializeField] private float damageDelay = 1.0f;

    /// <summary>
    /// The color to use when player is damaged
    /// </summary>
    [SerializeField] private Color damageColor;

    /// <summary>
    /// How long we should change the color before we change it back (seconds)
    /// </summary>
    [SerializeField] private float changeColorDuration = 0.25f;

    /// <summary>
    /// Sound we should play if we get damaged
    /// </summary>
    [SerializeField] private AudioClip damageSound = null;

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

    /// <summary>
    /// Our current health
    /// </summary>
    private int currentHealth = 0;

    /// <summary>
    /// The last time we were hit
    /// </summary>
    private float lastHit = 0;

    /// <summary>
    /// Our original color
    /// </summary>
    private Color originalColor;

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
        // NOTE: I referenced the following code from http://wiki.unity3d.com/index.php?title=LookAtMouse
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
            gameController.SpawnProjectile(this.transform.position, targetRotation, this.tag, "Enemy", damage, false);
        }
    }

    /// <summary>
    /// Changes the player's current health
    /// </summary>
    /// <param name="value">The amount we want to change the player's health by</param>
    /// <param name="overHeal">Flag if we should heal past the max health</param>
    private void ChangeHealth(int value, bool overHeal = false)
    {
        currentHealth += value;

        // If we don't want to heal past max health, cap the healing to the max health value
        if (!overHeal && currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        // Let game controller know our health changed, so we can update the UI
        gameController.SendMessage("OnHealthChange", currentHealth);
    }

    /// <summary>
    /// Handles changing the player's damage
    /// </summary>
    /// <param name="value">The amount we want to increase our damage by</param>
    private void ChangeDamage(int value)
    {
        damage += value;
    }

    /// <summary>
    /// Apply color changes to the player
    /// </summary>
    /// <param name="color">The color to change to</param>
    /// <param name="delay">How many seconds we should wait before changing back</param>
    /// <returns>An IEnumerator to process a coroutine</returns>
    private IEnumerator ChangeColor(Color color, float delay)
    {
        playerRenderer.material.color = color;
        yield return new WaitForSeconds(delay);
        playerRenderer.material.color = originalColor;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        playerRigidbody = this.GetComponent<Rigidbody>();
        playerRenderer = this.GetComponentInChildren<Renderer>();
        gameController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameController>();
        currentHealth = maxHealth;
        originalColor = playerRenderer.material.color;

        // Let game controller update UI for health
        gameController.SendMessage("OnHealthChange", currentHealth);
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

    /// <summary>
    /// Handle trigger events
    /// </summary>
    /// <param name="other">The object that we triggered or triggered us</param>
    private void OnTriggerEnter(Collider other)
    {
        // If we reach the goal, notify the game controller that we got to the finish
        if (other.gameObject.tag == "Finish")
        {
            gameController.SendMessage("OnFinish");
        }
    }

    /// <summary>
    /// Decide what to do when enemy hits player
    /// </summary>
    /// <param name="damage">The amount of damage to apply</param>
    private void OnHit(int damage)
    {
        float now = Time.time;
        // If the player is not invincible at this time, apply damage, change color, and play sound
        if (now >= lastHit + damageDelay)
        {
            lastHit = now;
            ChangeHealth(-damage);
            StartCoroutine(ChangeColor(damageColor, changeColorDuration));
        }
    }

    /// <summary>
    /// Decide what to do if we are "healed"
    /// </summary>
    /// <param name="value">How much we should be healed</param>
    private void OnHeal(int value)
    {
        ChangeHealth(value);
    }

    /// <summary>
    /// Decide what to do if we get a damage increase
    /// </summary>
    /// <param name="value">The amount we want to increase our damage by</param>
    private void OnDamageIncrease(int value)
    {
        ChangeDamage(value);
    }

    /// <summary>
    /// Handle what happens when we get shot
    /// </summary>
    /// <param name="damage">The amount of damage we might take</param>
    private void OnShot(int damage)
    {
        // Don't apply damage if we are in invincibility frame
        float now = Time.time;
        if (now < lastHit + damageDelay) return;
        lastHit = now;

        // Apply damage and change color accordingly
        ChangeHealth(-damage);
        StartCoroutine(ChangeColor(damageColor, changeColorDuration));

        // Play damage sound
        AudioSource.PlayClipAtPoint(damageSound, Camera.main.transform.position);
    }

    #endregion
}
