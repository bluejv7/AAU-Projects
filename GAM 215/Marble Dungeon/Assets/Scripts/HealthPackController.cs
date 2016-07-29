using UnityEngine;
using System.Collections;

/// <summary>
/// Handles events and movements for the health pack
/// </summary>
public class HealthPackController : MonoBehaviour {
    /// <summary>
    /// How much damage to heal (should only apply to current health and should cap at max health)
    /// </summary>
    [SerializeField] private int damageHealed = 3;

    /// <summary>
    /// The rotation angle
    /// </summary>
    [SerializeField] private float rotationAngle = 2.0f;

    /// <summary>
    /// Our last rotation
    /// </summary>
    private Vector3 previousRotation = Vector3.zero;

    /// <summary>
    /// Initialize variables
    /// </summary>
    private void Start()
    {
        previousRotation = this.transform.eulerAngles;
    }

    /// <summary>
    /// Rotate the health pack
    /// </summary>
    private void Rotate()
    {
        previousRotation.y += rotationAngle;
        this.transform.eulerAngles = previousRotation;
    }

	/// <summary>
    /// Handle what we should do every frame
    /// </summary>
	private void Update()
    {
        Rotate();
	}

    /// <summary>
    /// Handle to do when someone triggers us or we trigger them
    /// </summary>
    /// <param name="other">The object triggered</param>
    private void OnTriggerEnter(Collider other)
    {
        // Let player refill on health
        if (other.gameObject.tag == "Player")
        {
            other.SendMessage("OnHeal", damageHealed);
            this.transform.DetachChildren();
            GameObject.Destroy(this.gameObject);
        }
    }
}
