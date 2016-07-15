using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Handles some initialization that only the game should know about
/// </summary>
public class GameController : MonoBehaviour {
    /// <summary>
    /// Reference to the base health text object
    /// </summary>
    [SerializeField] private Text baseHealthText = null;
    
    /// <summary>
    /// Reference to end game text object
    /// </summary>
    [SerializeField] private Text endGameText = null;

    /// <summary>
    /// The prefix we'll use for the base health text before putting in the value
    /// </summary>
    [SerializeField] private string baseHealthTextPrefix = "Base Health: ";

    /// <summary>
    /// The current base health
    /// </summary>
    [SerializeField] private int baseHealth = 100;

    /// <summary>
    /// Tells us if we have won or not
    /// </summary>
    private bool hasWon = false;

    /// <summary>
    /// Perform clean up tasks when the game is lost
    /// </summary>
    private void LoseGame()
    {
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().Die();
        endGameText.text = "You Lose!";
    }

    /// <summary>
    /// Notify player when we win and disable lose condition
    /// </summary>
    private void WinGame()
    {
        hasWon = true;
        endGameText.text = "You Win!";
    }

	/// <summary>
    /// Initialize some variables or starting game state
    /// </summary>
	private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
	}

    /// <summary>
    /// Applies damage to the base health
    /// </summary>
    /// <param name="damage">The damage to apply</param>
    public void DamageBase(int damage)
    {
        // Apply damaage and update text
        baseHealth -= damage;
        baseHealthText.text = baseHealthTextPrefix + baseHealth;

        // If base health is gone, activate lose scenario
        if (baseHealth <= 0 && !hasWon)
        {
            LoseGame();
        }
    }
}
