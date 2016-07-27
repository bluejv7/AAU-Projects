using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Handles overall game logic that doesn't tie closely with other objects (win/lose conditions, spawning, and more)
/// </summary>
public class GameController : MonoBehaviour {
    #region Game Properties

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
    public void SpawnProjectile(Vector3 spawnPosition, Quaternion rotation, string spawnTag, string damageTag, int damage)
    {
        // Get a projectile and give it the correct position/rotation
        GameObject _projectile = PopPool();
        _projectile.transform.position = spawnPosition;
        _projectile.transform.rotation = rotation;

        // Modify some other properties of the bullet for collision purposes
        BulletController bulletController = _projectile.GetComponent<BulletController>();
        bulletController.damage = damage;
        bulletController.damageTag = "Enemy";
        bulletController.spawnTag = spawnTag;

        // This initialization makes sure proper values are being set upon enabling the projectile
        bulletController.initialize();

        // Enable projectile
        _projectile.SetActive(true);
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
	}

    #endregion
}
