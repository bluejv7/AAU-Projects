using UnityEngine;
using System.Collections;

/// <summary>
/// Handles what should happen with the story button events
/// </summary>
public class StoryButtonController : MonoBehaviour {
    /// <summary>
    /// Reference to the story panel
    /// </summary>
    [SerializeField] private GameObject storyPanel = null;

    /// <summary>
    /// Show the story panel
    /// </summary>
    public void ShowStory()
    {
        storyPanel.SetActive(true);
    }
}
