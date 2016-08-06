using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/// <summary>
/// Handle actions for the start button
/// </summary>
public class StartButtonController : MonoBehaviour {
    /// <summary>
    /// Loads the game scene
    /// </summary>
    public void StartGame()
    {
        SceneManager.LoadScene("Game Scene");
    }
}
