using UnityEngine;
using System.Collections;

/// <summary>
/// Specifies movement event triggers for the drone
/// </summary>
public class DroneController : MonoBehaviour {
    /// <summary>
    /// The damage the drone will try to impart upon the player
    /// </summary>
    [SerializeField] private int damage = 10;

    /// <summary>
    /// The speed at which the drone will move
    /// </summary>
    [SerializeField] private float speed = 4.0f;

    /// <summary>
    /// The angle at which the drone patrols around
    /// </summary>
    [SerializeField] private float angle = -1.0f;
	
	/// <summary>
    /// Update the movement of the drone
    /// </summary>
	private void Update ()
    {
        // We're considering "right" as forward, since the model I created is facing the wrong way... =\
        this.transform.Translate(this.transform.right * Time.deltaTime * speed);
        this.transform.Rotate(0, angle, 0);
	}

    /// <summary>
    /// Apply damage to player if triggered by player
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<CharacterRigidBodyController>().Damage(damage);
        }
    }
}
