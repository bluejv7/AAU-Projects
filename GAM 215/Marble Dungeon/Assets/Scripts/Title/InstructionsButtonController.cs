using UnityEngine;
using System.Collections;

/// <summary>
/// Handles events for the instructions button
/// </summary>
public class InstructionsButtonController : MonoBehaviour {
    /// <summary>
    /// Reference to the instructions panel
    /// </summary>
    [SerializeField] private GameObject instructionsPanel = null;

    /// <summary>
    /// Show instructions on the canvas
    /// </summary>
    public void ShowInstructions()
    {
        instructionsPanel.SetActive(true);
    }
}
