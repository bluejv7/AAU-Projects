using UnityEngine;
using System.Collections;

/// <summary>
/// Handles events and movements for the damage boost
/// </summary>
public class DamageBoostController : MonoBehaviour
{
    /// <summary>
    /// How much to increase for damage
    /// </summary>
    [SerializeField] private int damageIncrease = 1;

    /// <summary>
    /// The rotation angle
    /// </summary>
    [SerializeField] private float rotationAngle = 2.0f;

    /// <summary>
    /// Sound when something activates the damage boost
    /// </summary>
    [SerializeField] private AudioClip damageBoostSound = null;

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
    /// Handle rotating our power up
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
        // Let player power up their damage
        if (other.gameObject.tag == "Player")
        {
            other.SendMessage("OnDamageIncrease", damageIncrease);

            // Play damage boost sound
            AudioSource.PlayClipAtPoint(damageBoostSound, Camera.main.transform.position);

            // Destroy self
            this.transform.DetachChildren();
            GameObject.Destroy(this.gameObject);
        }
    }
}
