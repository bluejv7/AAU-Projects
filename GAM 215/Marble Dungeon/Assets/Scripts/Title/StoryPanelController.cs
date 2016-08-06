using UnityEngine;
using System.Collections;

/// <summary>
/// Handle events for story panel
/// </summary>
public class StoryPanelController : MonoBehaviour {
    /// <summary>
    /// Handle what happens every frame
    /// </summary>
	private void Update()
    {
        // If user clicks anywhere, disable this panel
        if (Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
        }
    }
}
