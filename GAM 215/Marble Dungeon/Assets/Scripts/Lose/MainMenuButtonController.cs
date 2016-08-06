using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenuButtonController : MonoBehaviour {
    /// <summary>
    /// Open the main menu
    /// </summary>
    public void OpenMainMenu()
    {
        SceneManager.LoadScene("Title Scene");
    }
}
