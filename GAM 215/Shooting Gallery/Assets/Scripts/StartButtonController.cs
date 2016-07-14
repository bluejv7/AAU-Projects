using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Contains functions that can be triggered from button events (like onclick)
/// </summary>
public class StartButtonController : MonoBehaviour {
    /// <summary>
    /// Loads the game scene
    /// </summary>
    public void LoadGame()
    {
        SceneManager.LoadScene("Scenes/Game Scene");
    }
}
