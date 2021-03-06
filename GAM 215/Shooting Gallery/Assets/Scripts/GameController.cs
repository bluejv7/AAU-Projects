﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Handles some initialization that only the game should know about
/// </summary>
public class GameController : MonoBehaviour {
    #region Object References

    [Header("--- Object References ---")]

    /// <summary>
    /// Default enemy to populate if we don't have enough enemies for the waves
    /// </summary>
    [SerializeField] private GameObject defaultEnemy = null;

    /// <summary>
    /// Array of enemies for each wave (wave 1 enemy is index 0 or element 1 of the array)
    /// </summary>
    [SerializeField] private List<GameObject> waveEnemies = new List<GameObject>();

    /// <summary>
    /// Array of points where we'll spawn enemies
    /// </summary>
    [SerializeField] private Transform[] spawnPoints;

    /// <summary>
    /// Reference to the base health text object
    /// </summary>
    [SerializeField] private Text baseHealthText = null;

    /// <summary>
    /// Reference to the wave text object
    /// </summary>
    [SerializeField] private Text waveText = null;
    
    /// <summary>
    /// Reference to end game text object
    /// </summary>
    [SerializeField] private Text endGameText = null;

    #endregion

    #region Text Config

    [Header("--- Text Config ---")]

    /// <summary>
    /// The prefix we'll use for the base health text before putting in the value
    /// </summary>
    [SerializeField] private string baseHealthTextPrefix = "Base Health: ";

    /// <summary>
    /// The prefix we'll use for the wave text before putting in the value
    /// </summary>
    [SerializeField] private string waveTextPrefix = "Wave ";

    #endregion

    #region Game Config

    [Header("--- Game Config ---")]

    /// <summary>
    /// The current base health
    /// </summary>
    [SerializeField] private int baseHealth = 100;

    /// <summary>
    /// The amount of waves we'll have in this game
    /// </summary>
    [SerializeField] private int waves = 3;

    /// <summary>
    /// How many enemies we'll spawn per wave
    /// </summary>
    [SerializeField] private int enemiesPerWave = 5;

    #endregion

    #region Calculation Variables

    /// <summary>
    /// Tells us if we have won or not
    /// </summary>
    private bool hasWon = false;

    /// <summary>
    /// The current wave we're on
    /// </summary>
    private int currentWave = 0;

    /// <summary>
    /// The number of enemies destroyed this wave
    /// </summary>
    private int numEnemiesDestroyedThisWave = 0;

    #endregion

    #region Helper Functions

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

    /// <summary>
    /// Start spawning enemies for the next wave
    /// </summary>
    public IEnumerator StartWave()
    {
        // Reset enemies destroyed this wave
        numEnemiesDestroyedThisWave = 0;

        // Increase counter for the current wave (we started at 0)
        currentWave++;

        // If we are past the max number of waves, we win!
        if (currentWave > waves)
        {
            WinGame();
            yield break;
        }

        // Update wave text (didn't want to update it if we won the game)
        waveText.text = waveTextPrefix + currentWave;

        int numSpawnPoints = spawnPoints.Length;
        GameObject enemy = waveEnemies[currentWave - 1];

        // Start spawning enemies
        for (int i = 0; i < enemiesPerWave; i++)
        {
            int spawnIndex = Random.Range(0, numSpawnPoints - 1);
            Instantiate(enemy, spawnPoints[spawnIndex].position, Quaternion.identity);
            yield return new WaitForSeconds(0.25f);
        }

        // This is in case there is a race condition for enemy death event trying to start the wave and failing
        // ...hopefully this doesn't start two waves in a row, but should be unlikely?
        if (ShouldStartWave())
        {
            StartCoroutine(StartWave());
        }
    }

    /// <summary>
    /// Check to see if we should start spawning waves
    /// </summary>
    private bool ShouldStartWave()
    {
        return numEnemiesDestroyedThisWave >= enemiesPerWave;
    }

    #endregion

    #region Event Handlers

    /// <summary>
    /// Initialize some variables or starting game state
    /// </summary>
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Populate the wave enemies if we are still missing some
        int numMissing = waves - waveEnemies.Count;
        for (int i = 0; i < numMissing; i++)
        {
            waveEnemies.Add(defaultEnemy);
        }

        // Start a wave
        StartCoroutine(StartWave());
	}

    /// <summary>
    /// When an enemy dies, start new wave if no enemies are left
    /// </summary>
    private void OnEnemyDeath()
    {
        numEnemiesDestroyedThisWave++;

        // If the last enemy dies, we should start a new wave -- don't start a new wave while we are spawning
        // Note: Previously, I had wanted to avoid keeping a counter on how many enemies were destroyed per wave, but it made it
        // somwhat hard to eliminate race conditions with StartWave() coroutines... so I think I'll utilize a function to handle
        // this check in case the check becomes more complicated and get modified by what type of enemy appears
        if (ShouldStartWave())
        {
            StartCoroutine(StartWave());
        }
    }

    #endregion
}
