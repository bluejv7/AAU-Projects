using UnityEngine;
using System.Collections;

/// <summary>
/// Define logic for bullet events/triggers
/// </summary>
public class BulletController : MonoBehaviour {
    #region Bullet Configs

    /// <summary>
    /// Our speed moving forward
    /// </summary>
    [SerializeField] private float speed = 10.0f;

    /// <summary>
    /// The damage we deal
    /// </summary>
    [SerializeField] private int damage = 1;

    /// <summary>
    /// The limits of where the bullet can travel (values are for both +/- x/y/z)
    /// </summary>
    [SerializeField] private Vector3 boundaryLimits = new Vector3(50.0f, 50.0f, 50.0f);

    #endregion

    #region Calculation Variables

    /// <summary>
    /// The position we had previously
    /// </summary>
    private Vector3 previousPosition = Vector3.zero;

    /// <summary>
    /// Our current position
    /// </summary>
    private Vector3 currentPosition = Vector3.zero;

    /// <summary>
    /// The vector3 difference between previous and current position
    /// </summary>
    private Vector3 movementThisStep = Vector3.zero;

    /// <summary>
    /// Calculation for distanced traveled from previous position to current position
    /// </summary>
    private float distanceTraveled = 0;

    /// <summary>
    /// Variable for anything we hit during a raycast
    /// </summary>
    private RaycastHit hitInfo;

    /// <summary>
    /// The collider we impacted
    /// </summary>
    private Collider hitCollider = null;

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        previousPosition = this.transform.position;
        currentPosition = previousPosition;
	}
	
	/// <summary>
    /// Handle movement every frame
    /// </summary>
	private void Update()
    {
        Move();
        CheckForCollision();

        // Update previous position
        previousPosition = currentPosition;

        // Destroy if out of bounds
        if (IsOutOfBounds())
        {
            GameObject.Destroy(this.gameObject);
        }
	}

    /// <summary>
    /// Handle necessary actions before destroying self
    /// </summary>
    private void OnDestroy()
    {
        // Take care of children -- haven't made bullets with children yet, but might happen at some point!
        this.transform.DetachChildren();

        // Tell parent we are destroying ourselves -- kind of sad ='(
        // This is useful for parents that contains a certain amount of children and need to destroy itself after its children are gone
        if (this.transform.parent)
        {
            this.transform.parent.SendMessage("OnChildDestroy", SendMessageOptions.DontRequireReceiver);
        }
    }

    #endregion

    #region Helper Functions

    /// <summary>
    /// Calculate if we hit anything using raycasts
    /// (referenced http://wiki.unity3d.com/index.php?title=DontGoThroughThings, but I'm still doing things my way)
    /// </summary>
    private void CheckForCollision()
    {
        // Get the distance traveled to implement in our raycast
        movementThisStep = previousPosition - currentPosition;
        distanceTraveled = Mathf.Sqrt(movementThisStep.sqrMagnitude);

        // Cast the ray
        bool didHit = Physics.Raycast(previousPosition, movementThisStep, out hitInfo, distanceTraveled);

        // If we hit something...
        if (didHit)
        {
            hitCollider = hitInfo.collider;

            // Trigger OnShot event for enemies
            if (hitCollider.gameObject.tag == "Enemy")
            {
                hitCollider.SendMessage("OnShot", damage);
            }

            // Destroy ourselves
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// Move our bullet
    /// </summary>
    private void Move()
    {
        this.transform.Translate(Vector3.forward * speed * Time.deltaTime);
        currentPosition = this.transform.position;
    }

    /// <summary>
    /// Check if bullet goes past our boundary limits
    /// </summary>
    /// <returns></returns>
    private bool IsOutOfBounds()
    {
        return Mathf.Abs(currentPosition.x) >= boundaryLimits.x || Mathf.Abs(currentPosition.y) >= boundaryLimits.y || Mathf.Abs(currentPosition.z) >= boundaryLimits.z;
    }

    #endregion
}
