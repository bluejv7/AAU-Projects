using UnityEngine;
using System.Collections;

/// <summary>
/// Defines the logic for spread shot events
/// </summary>
public class SpreadShotController : MonoBehaviour {
    /// <summary>
    /// The number of bullets in a spread shot
    /// (This is not configurable until I figure out how to spawn and attach children)
    /// </summary>
    private int numBullets = 5;

    /// <summary>
    /// Decrement number of bullets in the spread shot and if it is less than or equal to zero, destroy this object
    /// </summary>
    private void OnChildDestroy()
    {
        // Decrement bullets
        numBullets--;

        // Destroy object
        if (numBullets <= 0)
        {
            // Note: Not detaching children, since they are garbage collecting themselves
            // (also, when I tried that, my game froze...)
            GameObject.Destroy(this.gameObject);
        }
    }
}
