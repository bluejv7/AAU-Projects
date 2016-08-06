using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

/// <summary>
/// Handles overall game logic that doesn't tie closely with other objects (win/lose conditions, spawning, and more)
/// </summary>
public class GameController : MonoBehaviour {
    #region Reference Variables

    /// <summary>
    /// Reference to ending text
    /// </summary>
    [SerializeField] private Text endText = null;

    /// <summary>
    /// Reference to health text
    /// </summary>
    [SerializeField] private Text healthText = null;

    /// <summary>
    /// Reference to the final wall before the goal
    /// </summary>
    [SerializeField] private GameObject finalWall = null;

    /// <summary>
    /// Reference to the final entrance near the goal
    /// </summary>
    [SerializeField] private GameObject finalEntrance = null;

    #endregion

    #region Game Properties

    /// <summary>
    /// Message to display when player wins
    /// </summary>
    [SerializeField] private string winMessage = "You Win!";

    /// <summary>
    /// Message to display when player loses
    /// </summary>
    [SerializeField] private string loseMessage = "You Lose! =(";

    /// <summary>
    /// The prefix for the health text
    /// </summary>
    [SerializeField] private string healthTextPrefix = "Health: ";

    /// <summary>
    /// List of projectiles we'll distribute and collect
    /// </summary>
    private List<GameObject> projectilePool = new List<GameObject>();

    /// <summary>
    /// Projectile to use for the pool
    /// </summary>
    [SerializeField] private GameObject projectile = null;

    /// <summary>
    /// The amount of projectiles to prefill the pool with
    /// </summary>
    [SerializeField] private int initialPoolSize = 5;

    /// <summary>
    /// Sound to play when spawning projectiles
    /// </summary>
    [SerializeField] private AudioClip firingSound = null;

    /// <summary>
    /// Sound to play when we win
    /// </summary>
    [SerializeField] private AudioClip victorySound = null;

    #endregion

    #region Calculation Variables

    /// <summary>
    /// Flag for if we won
    /// </summary>
    private bool hasWon = false;

    #endregion

    #region Helper Functions

    /// <summary>
    /// Add more projectiles to the pool
    /// </summary>
    public void PushPool(GameObject projectile = null)
    {
        GameObject _projectile = null;
        // If a projectile is given, disable it and use that for the pool
        if (projectile)
        {
            projectile.SetActive(false);
            _projectile = projectile;
        }
        // Otherwise, create a new projectile
        else
        {
            _projectile = Instantiate(this.projectile, Vector3.zero, Quaternion.identity) as GameObject;
        }
        projectilePool.Add(_projectile);
    }

    /// <summary>
    /// Gives out a projectile from the pool (or creates a new one to give out)
    /// </summary>
    /// <returns>Returns the first pool element</returns>
    private GameObject PopPool()
    {
        GameObject _projectile = null;
        // Add more projectiles to the pool if we're missing some
        if (projectilePool.Count == 0)
        {
            _projectile = Instantiate(this.projectile, Vector3.zero, Quaternion.identity) as GameObject;
            return _projectile;
        }

        // Otherwise, grab from the pool
        _projectile = projectilePool[0];
        projectilePool.RemoveAt(0);
        return _projectile;
    }

    /// <summary>
    /// Grab projectiles from the pool and place it into the world
    /// </summary>
    /// <param name="spawnPosition">Where we want to spawn the projectile</param>
    /// <param name="rotation">Where the projectile should face</param>
    /// <param name="spawnTag">The tag that spawned the projectile (so we don't collide with the creator)</param>
    /// <param name="damageTag">The object tag we want to damage</param>
    /// <param name="damage">The amount of damage we want to inflict</param>
    public void SpawnProjectile(Vector3 spawnPosition, Quaternion rotation, string spawnTag, string damageTag, int damage, bool collideWithSelf = true)
    {
        // Get a projectile and give it the correct position/rotation
        GameObject _projectile = PopPool();
        _projectile.transform.position = spawnPosition;
        _projectile.transform.rotation = rotation;

        // Modify some other properties of the bullet for collision purposes
        BulletController bulletController = _projectile.GetComponent<BulletController>();
        bulletController.damage = damage;
        bulletController.damageTag = damageTag;
        bulletController.spawnTag = spawnTag;
        bulletController.collideWithSelf = collideWithSelf;

        // This initialization makes sure proper values are being set upon enabling the projectile
        bulletController.initialize();

        // Play the firing sound
        AudioSource.PlayClipAtPoint(firingSound, _projectile.transform.position);

        // Enable projectile
        _projectile.SetActive(true);
    }

    /// <summary>
    /// Do everything to notify player that they've won!
    /// </summary>
    private void WinGame()
    {
        // TODO: This block of code might be useless due to the win scene loading, but maybe I'll make the win scene go back to this one
        hasWon = true;
        endText.text = winMessage;

        // Load win scene
        SceneManager.LoadScene("Win Scene");
    }

    /// <summary>
    /// Do everything to notify player that they've lost... =(
    /// </summary>
    private void LoseGame()
    {
        endText.text = loseMessage;

        // Note: Thought about telling the player to destroy itself, but the game controller should be able to step in whenever it feels it needs to
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.transform.DetachChildren();
        GameObject.Destroy(player);

        // Load lose scene
        SceneManager.LoadScene("Lose Scene");
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        // Initialize pool with the number of projectiles we want to start with
        projectile.SetActive(false);
	    for(int i = 0; i < initialPoolSize; i++)
        {
            PushPool();
        }

        finalWall = GameObject.FindWithTag("Final Wall");
        finalEntrance = GameObject.FindWithTag("Final Entrance");
	}

    /// <summary>
    /// Decide what to do on a finish event
    /// </summary>
    private void OnFinish()
    {
        WinGame();
        AudioSource.PlayClipAtPoint(victorySound, Camera.main.transform.position);
    }

    /// <summary>
    /// Handle event for player health change
    /// </summary>
    /// <param name="currentHealth">Player's current health</param>
    private void OnHealthChange(int currentHealth)
    {
        // Update health text
        healthText.text = healthTextPrefix + currentHealth;

        // If our health is 0 or below, we lose... destroy player object
        if (currentHealth <= 0 && !hasWon)
        {
            LoseGame();
        }
    }

    /// <summary>
    /// When boss dies, do special actions
    /// </summary>
    private void OnBossDeath()
    {
        // Remove last barrier to goal
        finalWall.SetActive(false);

        // Color final entrance green
        Color green = new Color(0, 1, 0);
        Renderer[] wallRenderers = finalEntrance.GetComponentsInChildren<Renderer>();
        foreach (Renderer wallRenderer in wallRenderers)
        {
            wallRenderer.material.color = green;
        }
    }

    #endregion
}
