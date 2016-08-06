using UnityEngine;
using System.Collections;

/// <summary>
/// Handles events for instructions panel
/// </summary>
public class InstructionsPanelController : MonoBehaviour {

	/// <summary>
    /// Handle what happens every frame
    /// </summary>
	private void Update()
    {
	    // If user clicks anywhere, hide the instructions
        if (Input.GetMouseButtonDown(0))
        {
            this.gameObject.SetActive(false);
        }
	}
}
