using UnityEngine;
using System.Collections;

public class PyramidObstacle : MonoBehaviour {
    // serialized private variables
    [SerializeField] private int damage = 50;

    private void Start()
    {
    }

    // public methods

    /// <summary>
    /// Allows other methods to find out how much damage this object should deal
    /// </summary>
    /// <returns>Amount of damage object should deal</returns>
    public int GetDamage()
    {
        return damage;
    }

    /// <summary>
    /// Upon player collision, attempt to apply damage
    /// </summary>
    /// <param name="other">The collider that triggered this event</param>
    private void OnTriggerEnter(Collider other)
    {
        // Only try to apply damage if it is the player object
        if (other.tag == "Player")
        {
            // Get the player class and use the Damage public function to apply damage
            CharacterRigidBodyController player = other.gameObject.GetComponent<CharacterRigidBodyController>();
            player.Damage(damage);
        }
    }
}
